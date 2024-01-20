using Application.Features.Announcements.Constants;
using Application.Features.Languages.Constants;
using Application.Features.Languages.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;

namespace Application.Features.Languages.Rules;

public class LanguageBusinessRules : BaseBusinessRules
{
    private readonly ILanguageRepository _languageRepository;

    public LanguageBusinessRules(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public Task LanguageShouldExistWhenSelected(Language? language)
    {
        if (language == null)
            throw new BusinessException(LanguagesBusinessMessages.LanguageNotExists);
        return Task.CompletedTask;
    }

    public async Task LanguageIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Language? language = await _languageRepository.GetAsync(
            predicate: l => l.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await LanguageShouldExistWhenSelected(language);
    }

    public async Task LanguageShouldNotExistsWhenInsert(string name)
    {
        bool doesExists = await _languageRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(LanguagesBusinessMessages.LanguageNameExists);
    }
    public async Task LanguageShouldNotExistsWhenUpdate(string name)
    {
        bool doesExists = await _languageRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(LanguagesBusinessMessages.LanguageNameExists);
    }


}