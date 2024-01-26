using Application.Features.StudentExperiences.Constants;
using Application.Features.StudentExperiences.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.StudentExperiences.Constants.StudentExperiencesOperationClaims;
using Application.Services.ContextOperations;

namespace Application.Features.StudentExperiences.Commands.Update;

public class UpdateStudentExperienceCommand : IRequest<UpdatedStudentExperienceResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public Guid? StudentId { get; set; }
    public string CompanyName { get; set; }
    public string Sector { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public Guid CityId { get; set; }

    public string[] Roles => new[] { Admin, Write, StudentExperiencesOperationClaims.Update, "Student" };

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string CacheGroupKey => "GetStudentExperiences";

    public class UpdateStudentExperienceCommandHandler : IRequestHandler<UpdateStudentExperienceCommand, UpdatedStudentExperienceResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentExperienceRepository _studentExperienceRepository;
        private readonly StudentExperienceBusinessRules _studentExperienceBusinessRules;
        private readonly IContextOperationService _contextOperationService;
        public UpdateStudentExperienceCommandHandler(IMapper mapper, IStudentExperienceRepository studentExperienceRepository,
                                         StudentExperienceBusinessRules studentExperienceBusinessRules, IContextOperationService contextOperationService)
        {
            _mapper = mapper;
            _studentExperienceRepository = studentExperienceRepository;
            _studentExperienceBusinessRules = studentExperienceBusinessRules;
            _contextOperationService = contextOperationService;
        }

        public async Task<UpdatedStudentExperienceResponse> Handle(UpdateStudentExperienceCommand request, CancellationToken cancellationToken)
        {
            Student getStudent = await _contextOperationService.GetStudentFromContext();
            StudentExperience? studentExperience = await _studentExperienceRepository.GetAsync(predicate: se => se.Id == request.Id, cancellationToken: cancellationToken);
            await _studentExperienceBusinessRules.StudentExperienceShouldExistWhenSelected(studentExperience);
            request.StudentId = getStudent.Id;
            studentExperience = _mapper.Map(request, studentExperience);

            await _studentExperienceRepository.UpdateAsync(studentExperience!);

            UpdatedStudentExperienceResponse response = _mapper.Map<UpdatedStudentExperienceResponse>(studentExperience);
            return response;
        }
    }
}