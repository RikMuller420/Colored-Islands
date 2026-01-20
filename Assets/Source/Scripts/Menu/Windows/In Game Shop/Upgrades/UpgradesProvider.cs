using System;
using System.Linq;

public class UpgradesProvider : IUpgradesData
{
    private PlayerDataProvider _playerData;
    private UpgradeSettings _upgradeSettings;

    public event Action<UpgradeType> Upgraded;

    public UpgradesProvider(PlayerDataProvider playerData, UpgradeSettings upgradeSettings)
    {
        _playerData = playerData;
        _upgradeSettings = upgradeSettings;
    }

    public int UpgradeStage(UpgradeType upgradeType) => _playerData.GetUpgradeStage(upgradeType);

    public int CalculateUpgradedGoldAmount(int baseGold) =>
                (int)(baseGold * UpgradeStageValue(UpgradeType.IncreaseRewards));

    public void AddUpgradeStage(UpgradeType upgradeType)
    {
        int upgradeStage = UpgradeStage(upgradeType);
        upgradeStage++;
        _playerData.SetUpgradeStage(upgradeType, upgradeStage);
        _playerData.Save();
        Upgraded?.Invoke(upgradeType);
    }

    public float UpgradeStageValue(UpgradeType upgradeType)
    {
        int upgradeStage = _playerData.GetUpgradeStage(upgradeType);
        UpgradeSettingsData upgrade = _upgradeSettings.Upgrades.FirstOrDefault(upgrade => upgrade.Type == upgradeType);

        return (upgradeStage == 0) ?
                upgrade.DefaultValue :
                upgrade.StageValues[upgradeStage - 1];

    }
}
