using System;

public class UpgradesProvider 
{
    private GameProgressStorage _gameProgressStorage;

    public event Action<UpgradeType> Upgraded;

    public UpgradesProvider(GameProgressStorage gameProgressStorage)
    {
        _gameProgressStorage = gameProgressStorage;
    }

    public int UpgradeStage(UpgradeType upgradeType) => _gameProgressStorage.GetUpgradeStage(upgradeType);

    public void AddUpgradeStage(UpgradeType upgradeType)
    {
        int upgradeStage = UpgradeStage(upgradeType);
        upgradeStage++;
        _gameProgressStorage.SetUpgradeStage(upgradeType, upgradeStage);
        _gameProgressStorage.Save();
        Upgraded?.Invoke(upgradeType);
    }
}
