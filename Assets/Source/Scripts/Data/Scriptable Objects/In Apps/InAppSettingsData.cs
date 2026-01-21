using System.Collections.Generic;
using SlimeGround.Menu.Windows.InAppPurchase;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.InApps
{
	[System.Serializable]
	public class InAppSettingsData 
	{
	    [SerializeField] private InAppType _inAppType;
	    [SerializeField] private string _id;
	    [SerializeField] private string _localizationKey;
	    [SerializeField] private GameObject _iconPrefab;
	    [SerializeField] private float _price = 10;
	    [SerializeField] private List<InAppBonus> _inAppBonuses = new();
	    [SerializeField] private int _earnWithAddViewCount = 8;

	    public InAppType Type => _inAppType;
	    public string Id => _id;
	    public string LocalizationKey => _localizationKey;
	    public GameObject IconPrefab => _iconPrefab;
	    public float Price => _price;
	    public IEnumerable<InAppBonus> InAppBonuses => _inAppBonuses;
	    public int EarnWithAddViewCount => _earnWithAddViewCount;
	}
}
