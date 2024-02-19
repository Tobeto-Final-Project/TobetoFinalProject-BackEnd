using Application.Features.StudentSocialMedias.Constants;
using Application.Features.StudentSocialMedias.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.StudentSocialMedias.Constants.StudentSocialMediasOperationClaims;
using Application.Services.ContextOperations;

namespace Application.Features.StudentSocialMedias.Commands.Create;

public class CreateStudentSocialMediaCommand : IRequest<CreatedStudentSocialMediaResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid? StudentId { get; set; }
    public Guid SocialMediaId { get; set; }
    public string MediaAccountUrl { get; set; }

    public string[] Roles => new[] { Admin, Write, StudentSocialMediasOperationClaims.Create, "Student" };

    public int? UserId { get; set; }

    public string CacheGroupKey => $"GetStudent{UserId}";
    public bool BypassCache { get; }
    public string? CacheKey { get; }

    public class CreateStudentSocialMediaCommandHandler : IRequestHandler<CreateStudentSocialMediaCommand, CreatedStudentSocialMediaResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStudentSocialMediaRepository _studentSocialMediaRepository;
        private readonly StudentSocialMediaBusinessRules _studentSocialMediaBusinessRules;
        private readonly IContextOperationService _contextOperationService;
        public CreateStudentSocialMediaCommandHandler(IMapper mapper, IStudentSocialMediaRepository studentSocialMediaRepository,
                                         StudentSocialMediaBusinessRules studentSocialMediaBusinessRules, IContextOperationService contextOperationService)
        {
            _mapper = mapper;
            _studentSocialMediaRepository = studentSocialMediaRepository;
            _studentSocialMediaBusinessRules = studentSocialMediaBusinessRules;
            _contextOperationService = contextOperationService;
        }

        public async Task<CreatedStudentSocialMediaResponse> Handle(CreateStudentSocialMediaCommand request, CancellationToken cancellationToken)
        {
            Student getStudent = await _contextOperationService.GetStudentFromContext();
            request.StudentId = getStudent.Id;
            StudentSocialMedia studentSocialMedia = _mapper.Map<StudentSocialMedia>(request);
            await _studentSocialMediaBusinessRules.StudentSocialMediaShouldNotExistsWhenInsert(studentSocialMedia);
            await _studentSocialMediaBusinessRules.StudentSocialMediaSelectionControl(cancellationToken);

            await _studentSocialMediaRepository.AddAsync(studentSocialMedia);

            CreatedStudentSocialMediaResponse response = _mapper.Map<CreatedStudentSocialMediaResponse>(studentSocialMedia);
            return response;
        }
    }
}