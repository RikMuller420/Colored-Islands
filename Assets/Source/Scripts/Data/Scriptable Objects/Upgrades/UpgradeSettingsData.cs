using System.Collections.Generic;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Upgrades
{
	[System.Serializable]
	public class UpgradeSettingsData
	{
	    [SerializeField] private UpgradeType _upgradeType;
	    [SerializeField] private int[] _goldPrices = new int[5]
	    {
	        200,
	        500,
	        1000,
	        2000,
	        5000
	    };
	    [SerializeField] private float _defaultValue = 1f;
	    [SerializeField] private float[] _stageValues = new float[5]
	    {
	        1,
	        2,
	        3,
	        4,
	        5
	    };

	    public UpgradeType Type => _upgradeType;
	    public IReadOnlyList<int> GoldPrices => _goldPrices;
	    public float DefaultValue => _defaultValue;
	    public IReadOnlyList<float> StageValues => _stageValues;
	}
}
