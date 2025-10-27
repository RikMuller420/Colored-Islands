using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageSettings", menuName = "Custom/LanguageSettings")]
public class LocalizationSettings : ScriptableObject
{
    [SerializeField] private LanguageData[] _languages;

    public ReadOnlyCollection<LanguageData> Languages => new ReadOnlyCollection<LanguageData>(_languages);
}
