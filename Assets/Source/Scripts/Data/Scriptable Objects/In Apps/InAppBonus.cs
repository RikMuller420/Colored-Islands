using UnityEngine;

[System.Serializable]
public class InAppBonus
{
    [SerializeField] private InAppBonusType _type;
    [SerializeField] private int _amount;

    public InAppBonusType Type => _type;
    public int Amount => _amount;
}
