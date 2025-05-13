using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lean.Localization;
using UnityEngine;

public class LanguageChanger : MonoBehaviour
{
    [SerializeField] private SliderButton _sliderButton;

    private ReadOnlyCollection<string> _languages;

    private Dictionary<string, string> _languagesNames = new()
    {
        { "English", "English" },
        { "Russian", "Русский" },
        { "Turkish", "Türkçe" }
    };

    private void Start()
    {
        _languages = LeanLocalization.CurrentLanguages.Keys.ToList().AsReadOnly();

        string currentLanguage = LeanLocalization.GetFirstCurrentLanguage();
        int currentIndex = _languages.IndexOf(currentLanguage);

        ReadOnlyCollection<string> orderedLanguageNames = _languages
                            .Where(key => _languagesNames.ContainsKey(key))
                            .Select(key => _languagesNames[key])
                            .ToList()
                            .AsReadOnly();

        _sliderButton.Initialize(orderedLanguageNames, currentIndex);
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
        LeanLocalization.SetCurrentLanguageAll(_languages[value]);
    }
}
