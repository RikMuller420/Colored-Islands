using UnityEngine;

[System.Serializable]
public class BoostSettingsData
{
    [SerializeField] private BoostType _boostType;
    [SerializeField] private GameObject _iconPrefab;
    [SerializeField] private int _goldPrice = 100;

    public BoostType Type => _boostType;
    public GameObject IconPrefab => _iconPrefab;
    public int GoldPrice => _goldPrice;
}
