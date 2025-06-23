using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BoostButtonContent
{
    [SerializeField] private BoostType _type;
    [SerializeField] private Button _button;

    public BoostType Type => _type;
    public Button Button => _button;
}
