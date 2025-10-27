using System.Collections.ObjectModel;
using System.Linq;
using Lean.Localization;
using UnityEngine;

public class LanguageChanger : MonoBehaviour
{
    private const Language FirstLanguage = Language.Russian;
    private const Language NonFirstLanguage = Language.English;

    [SerializeField] private SliderButton _sliderButton;

    private LocalizationSettings _localizationSettings;
    private GameProgressStorage _gameProgressStorage;
    private ReadOnlyCollection<string> _languages;

    public void Initialize(LocalizationSettings localizationSettings, GameProgressStorage gameProgressStorage,
                           LocalizationProvider localizationProvider)
    {
        _localizationSettings = localizationSettings;
        _gameProgressStorage = gameProgressStorage;
        _languages = _localizationSettings.Languages.Select(language => language.Name).ToList().AsReadOnly();
        int languageIndex = 0;

        if (_gameProgressStorage.IsLanguageSaved)
        {
            languageIndex = _localizationSettings.Languages
                                            .Select((language, index) => new { language.Language, Index = index })
                                            .Where(language => language.Language == _gameProgressStorage.Language)
                                            .Select(language => language.Index)
                                            .FirstOrDefault();
        }
        else
        {
            languageIndex = _localizationSettings.Languages
                                            .Select((language, index) => new { language.Key, Index = index })
                                            .Where(language => language.Key == localizationProvider.GetLanguageKey())
                                            .Select(language => language.Index)
                                            .FirstOrDefault();
            _gameProgressStorage.SetLanguage(_localizationSettings.Languages[languageIndex].Language);
            _gameProgressStorage.Save();
        }

        _sliderButton.Initialize(_languages, languageIndex);
        Language language = _localizationSettings.Languages[languageIndex].Language;

        if (language != FirstLanguage)
        {
            LeanLocalization.SetCurrentLanguageAll(language.ToString());
        }
        else
        {
            LeanLocalization.SetCurrentLanguageAll(NonFirstLanguage.ToString());
            LeanLocalization.SetCurrentLanguageAll(language.ToString());
        }

        enabled = true;
    }

    private void OnEnable()
    {
        _sliderButton.ValueChanged += ChangeLanguage;
    }

    private void OnDisable()
    {
        _sliderButton.ValueChanged -= ChangeLanguage;
    }

    private void ChangeLanguage(int value)
    {
        Language language = _localizationSettings.Languages[value].Language;
        LeanLocalization.SetCurrentLanguageAll(language.ToString());
        _gameProgressStorage.SetLanguage(language);
        _gameProgressStorage.Save();
    }
}
