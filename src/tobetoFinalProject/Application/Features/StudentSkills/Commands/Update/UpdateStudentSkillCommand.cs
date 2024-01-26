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

namespace Application.Features.StudentSkills.Commands.Update;

public class UpdateStudentSkillCommand : IRequest<UpdatedStudentSkillResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public Guid? StudentId { get; set; }
    public Guid SkillId { get; set; }

    public string[] Roles => new[] { Admin, Write, StudentSkillsOperationClaims.Update, "Student" };

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string CacheGroupKey => "GetStudentSkills";

    public class UpdateStudentSkillCommandHandler : IRequestHandler<UpdateStudentSkillCommand, UpdatedStudentSkillResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentSkillRepository _studentSkillRepository;
        private readonly StudentSkillBusinessRules _studentSkillBusinessRules;
        private readonly IContextOperationService _contextOperationService;
        public UpdateStudentSkillCommandHandler(IMapper mapper, IStudentSkillRepository studentSkillRepository,
                                         StudentSkillBusinessRules studentSkillBusinessRules, IContextOperationService contextOperationService)
        {
            _mapper = mapper;
            _studentSkillRepository = studentSkillRepository;
            _studentSkillBusinessRules = studentSkillBusinessRules;
            _contextOperationService = contextOperationService;
        }

        public async Task<UpdatedStudentSkillResponse> Handle(UpdateStudentSkillCommand request, CancellationToken cancellationToken)
        {
            Student getStudent = await _contextOperationService.GetStudentFromContext();
            request.StudentId = getStudent.Id;
            StudentSkill? studentSkill = await _studentSkillRepository.GetAsync(predicate: ss => ss.Id == request.Id, cancellationToken: cancellationToken);
            await _studentSkillBusinessRules.StudentSkillShouldExistWhenSelected(studentSkill);
            studentSkill = _mapper.Map(request, studentSkill);
            await _studentSkillBusinessRules.StudentSkillShouldNotExistsWhenUpdate(studentSkill.SkillId, studentSkill.StudentId);
            await _studentSkillRepository.UpdateAsync(studentSkill!);

            UpdatedStudentSkillResponse response = _mapper.Map<UpdatedStudentSkillResponse>(studentSkill);
            return response;
        }
    }
}