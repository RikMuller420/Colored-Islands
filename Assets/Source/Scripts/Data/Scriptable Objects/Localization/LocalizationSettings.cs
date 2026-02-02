using System.Collections.ObjectModel;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Localization
{
	[CreateAssetMenu(fileName = "LanguageSettings", menuName = "Custom/LanguageSettings")]
	public class LocalizationSettings : ScriptableObject
	{
	    [SerializeField] private LanguageData[] _languages;

	    public ReadOnlyCollection<LanguageData> Languages => new ReadOnlyCollection<LanguageData>(_languages);
	}
}
