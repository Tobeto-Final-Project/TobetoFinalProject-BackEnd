using Application.Features.StudentSocialMedias.Constants;
using Application.Features.StudentSocialMedias.Constants;
using Application.Services.CacheForMemory;
using Application.Services.ContextOperations;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Application.Features.StudentSocialMedias.Rules;

public class StudentSocialMediaBusinessRules : BaseBusinessRules
{
    private readonly IStudentSocialMediaRepository _studentSocialMediaRepository;
    private readonly ICacheMemoryService _cacheMemoryService;
    private readonly IContextOperationService _contextOperationService;

    public StudentSocialMediaBusinessRules(IStudentSocialMediaRepository studentSocialMediaRepository, ICacheMemoryService cacheMemoryService, IContextOperationService contextOperationService)
    {
        _studentSocialMediaRepository = studentSocialMediaRepository;
        _cacheMemoryService = cacheMemoryService;
        _contextOperationService = contextOperationService;
    }
    public async Task StudentSocialMediaShouldNotExistsWhenInsert(StudentSocialMedia studentSocialMedia)
    {
        bool doesExists = await _studentSocialMediaRepository
            .AnyAsync(predicate: se =>
            se.StudentId == studentSocialMedia.StudentId
            && se.SocialMediaId == studentSocialMedia.SocialMediaId
            && se.MediaAccountUrl == studentSocialMedia.MediaAccountUrl
            , enableTracking: false);
        if (doesExists)
            throw new BusinessException(StudentSocialMediasBusinessMessages.StudentSocialMediaAlreadyExists);
    }
    public async Task StudentSocialMediaShouldNotExistsWhenUpdate(StudentSocialMedia studentSocialMedia)
    {
        bool doesExists = await _studentSocialMediaRepository
            .AnyAsync(predicate: se =>
            se.StudentId == studentSocialMedia.StudentId
            && se.SocialMediaId == studentSocialMedia.SocialMediaId
            && se.MediaAccountUrl == studentSocialMedia.MediaAccountUrl
            , enableTracking: false);
        if (doesExists)
            throw new BusinessException(StudentSocialMediasBusinessMessages.StudentSocialMediaAlreadyExists);
    }
    public Task StudentSocialMediaShouldExistWhenSelected(StudentSocialMedia? studentSocialMedia)
    {
        if (studentSocialMedia == null)
            throw new BusinessException(StudentSocialMediasBusinessMessages.StudentSocialMediaNotExists);
        return Task.CompletedTask;
    }
   

    public async Task StudentSocialMediaIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        StudentSocialMedia? studentSocialMedia = await _studentSocialMediaRepository.GetAsync(
            predicate: ssm => ssm.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await StudentSocialMediaShouldExistWhenSelected(studentSocialMedia);
    }

    public async Task StudentSocialMediaSelectionControl(CancellationToken cancellationToken) 
    {
        Student getStudent = await _contextOperationService.GetStudentFromContext();

        IPaginate<StudentSocialMedia> studentSocialMedias = await _studentSocialMediaRepository.GetListAsync(
                predicate: s => s.StudentId == getStudent.Id,
                include: sc => sc.Include(sc => sc.SocialMedia),
                cancellationToken: cancellationToken
        );

        if (studentSocialMedias.Count == 3)
            throw new BusinessException(StudentSocialMediasBusinessMessages.MaxStudentSocialMediaCapacity);
    }
}