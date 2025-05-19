using System.Collections.Generic;
using UnityEngine;

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

    public UpgradeType Type => _upgradeType;
    public IReadOnlyList<int> GoldPrices => _goldPrices;
}
