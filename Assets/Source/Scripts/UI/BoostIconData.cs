using TMPro;
using UnityEngine;

[System.Serializable]
public class BoostIconData
{
    [SerializeField] private BoostType _boostType;
    [SerializeField] private GameObject _icon;
    [SerializeField] private TextMeshProUGUI _amountText;

    public BoostType Type => _boostType;
    public GameObject Icon => _icon;
    public TextMeshProUGUI AmountText => _amountText;
}
