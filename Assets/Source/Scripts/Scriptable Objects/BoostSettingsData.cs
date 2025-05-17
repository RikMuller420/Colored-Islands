using UnityEngine;

[System.Serializable]
public class BoostSettingsData
{
    [SerializeField] private BoostType _boostType; 
    [SerializeField] private int _goldPrice = 100;

    public BoostType Type => _boostType;
    public int GoldPrice => _goldPrice;
}
