using Application.Features.StudentSkills.Constants;
using Application.Features.StudentSkills.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.StudentSkills.Constants.StudentSkillsOperationClaims;
using Application.Services.ContextOperations;

namespace Application.Features.StudentSkills.Commands.Create;

public class CreateStudentSkillCommand : IRequest<CreatedStudentSkillResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid? StudentId { get; set; }
    public Guid SkillId { get; set; }

    public string[] Roles => new[] { Admin, Write, StudentSkillsOperationClaims.Create, "Student" };

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string CacheGroupKey => "GetStudentSkills";

    public class CreateStudentSkillCommandHandler : IRequestHandler<CreateStudentSkillCommand, CreatedStudentSkillResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentSkillRepository _studentSkillRepository;
        private readonly StudentSkillBusinessRules _studentSkillBusinessRules;
        private readonly IContextOperationService _contextOperationService;
        public CreateStudentSkillCommandHandler(IMapper mapper, IStudentSkillRepository studentSkillRepository,
                                         StudentSkillBusinessRules studentSkillBusinessRules, IContextOperationService contextOperationService)
        {
            _mapper = mapper;
            _studentSkillRepository = studentSkillRepository;
            _studentSkillBusinessRules = studentSkillBusinessRules;
            _contextOperationService = contextOperationService;
        }

        public async Task<CreatedStudentSkillResponse> Handle(CreateStudentSkillCommand request, CancellationToken cancellationToken)
        {
            Student getStudent = await _contextOperationService.GetStudentFromContext();
            request.StudentId = getStudent.Id;
            StudentSkill studentSkill = _mapper.Map<StudentSkill>(request);
            await _studentSkillBusinessRules.StudentSkillShouldNotExistsWhenInsert(studentSkill.SkillId, studentSkill.StudentId);
            await _studentSkillRepository.AddAsync(studentSkill);

            CreatedStudentSkillResponse response = _mapper.Map<CreatedStudentSkillResponse>(studentSkill);
            return response;
        }
    }
}