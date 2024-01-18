using Application.Features.Announcements.Constants;
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

    public Task LanguageShouldNotExist(Language? language)
    {
        if (language != null)
            throw new BusinessException(LanguagesBusinessMessages.LanguageNameNotExists);
        return Task.CompletedTask;
    }
    public async Task LanguageNameShouldNotExist(Language language, CancellationToken cancellationToken)
    {
        Language? controlLanguage = await _languageRepository.GetAsync(
            predicate: a => a.Name == language.Name,
            enableTracking: false, 
            cancellationToken: cancellationToken
            );
        await LanguageShouldNotExist(controlLanguage);
    }


}