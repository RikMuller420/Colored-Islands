using SlimeGround.Integration.Localization;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Localization
{
	[System.Serializable]
	public class LanguageData 
	{
	    [SerializeField] private Language _language;
	    [SerializeField] private string _name;
	    [SerializeField] private string _key;

	    public LanguageData(Language language, string name, string key)
	    {
	        _language = language;
	        _name = name;
	        _key = key;
	    }

	    public Language Language => _language;
	    public string Name => _name;
	    public string Key => _key;
	}
}
