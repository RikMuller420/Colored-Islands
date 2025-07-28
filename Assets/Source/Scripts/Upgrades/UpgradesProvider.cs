using System;
using System.Linq;

public class UpgradesProvider 
{
    private GameProgressStorage _gameProgressStorage;
    private UpgradeSettings _upgradeSettings;

    public event Action<UpgradeType> Upgraded;

    public UpgradesProvider(GameProgressStorage gameProgressStorage, UpgradeSettings upgradeSettings)
    {
        _gameProgressStorage = gameProgressStorage;
        _upgradeSettings = upgradeSettings;
    }

    public int UpgradeStage(UpgradeType upgradeType) => _gameProgressStorage.GetUpgradeStage(upgradeType);

    public int CalculateUpgradedGoldAmount(int baseGold) =>
                (int)(baseGold * UpgradeStageValue(UpgradeType.IncreaseRewards));

    public void AddUpgradeStage(UpgradeType upgradeType)
    {
        int upgradeStage = UpgradeStage(upgradeType);
        upgradeStage++;
        _gameProgressStorage.SetUpgradeStage(upgradeType, upgradeStage);
        _gameProgressStorage.Save();
        Upgraded?.Invoke(upgradeType);
    }

    public float UpgradeStageValue(UpgradeType upgradeType)
    {
        int upgradeStage = _gameProgressStorage.GetUpgradeStage(upgradeType);
        UpgradeSettingsData upgrade = _upgradeSettings.Upgrades.FirstOrDefault(upgrade => upgrade.Type == upgradeType);

        return (upgradeStage == 0) ?
                upgrade.DefaultValue :
                upgrade.StageValues[upgradeStage - 1];

    }
}
