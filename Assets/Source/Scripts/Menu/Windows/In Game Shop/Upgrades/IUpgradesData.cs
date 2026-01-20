using System;

public interface IUpgradesData
{
    public event Action<UpgradeType> Upgraded;

    public float UpgradeStageValue(UpgradeType upgradeType);
    public int UpgradeStage(UpgradeType upgradeType);
    public int CalculateUpgradedGoldAmount(int baseGold);
}
