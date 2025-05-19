using UnityEngine;

[System.Serializable]
public class BoostIconData
{
    [SerializeField] private BoostType _boostType;
    [SerializeField] private GameObject _icon;

    public BoostType Type => _boostType;
    public GameObject Icon => _icon;
}
