using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BoostButtonContent
{
    [SerializeField] private BoostType _type;
    [SerializeField] private BoostButton _button;
    [SerializeField] private List<Image> _buttonImages;
    [SerializeField] private GameObject _amountHolder;

    public BoostType Type => _type;
    public BoostButton Button => _button;
    public IReadOnlyCollection<Image> ButtonImages => _buttonImages;
    public GameObject AmountHolder => _amountHolder;
}
