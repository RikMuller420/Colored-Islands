using System.Collections.ObjectModel;
using System.Linq;
using Lean.Localization;
using UnityEngine;

public class LanguageChanger : MonoBehaviour
{
    private const Language FirstLanguage = Language.Russian;
    private const Language NonFirstLanguage = Language.English;

    [SerializeField] private LocalizationSettings _localizationSettings;
    [SerializeField] private PlayerDataProvider _playerData;
    [SerializeField] private SliderButton _sliderButton;

    private ReadOnlyCollection<string> _languages;

    public void Initialize()
    {
        var localizationProvider = new LocalizationProvider();
        _languages = _localizationSettings.Languages.Select(language => language.Name).ToList().AsReadOnly();
        int languageIndex = 0;

        if (_playerData.IsLanguageSaved)
        {
            languageIndex = _localizationSettings.Languages
                                            .Select((language, index) => new { language.Language, Index = index })
                                            .Where(language => language.Language == _playerData.Language)
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
            _playerData.SetLanguage(_localizationSettings.Languages[languageIndex].Language);
            _playerData.Save();
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
        _playerData.SetLanguage(language);
        _playerData.Save();
    }
}
