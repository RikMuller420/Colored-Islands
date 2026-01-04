using UnityEngine;

[System.Serializable]
public class LevelRewardData 
{
    [SerializeField] private int _levelId;
    [SerializeField] private int _rouletteSpinAmount = 1;
    [SerializeField] private int _goldAmount = 100;
    [SerializeField] private BoostType _boostType;
    [SerializeField] private int _boostAmount = 1;

    public int LevelId => _levelId;
    public int RouletteSpinAmount => _rouletteSpinAmount;
    public int GoldAmount => _goldAmount;
    public BoostType BoostType => _boostType;
    public int BoostAmount => _boostAmount;
}
