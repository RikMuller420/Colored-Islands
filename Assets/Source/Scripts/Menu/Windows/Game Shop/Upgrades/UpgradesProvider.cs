using System;
using System.Linq;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Upgrades;

namespace SlimeGround.Menu.Windows.GameShop.Upgrades
{
	public class UpgradesProvider : IUpgradesData
	{
	    private readonly PlayerDataProvider _playerData;
	    private readonly UpgradeSettings _upgradeSettings;

	    public UpgradesProvider(PlayerDataProvider playerData, UpgradeSettings upgradeSettings)
	    {
	        _playerData = playerData;
	        _upgradeSettings = upgradeSettings;
	    }

		public event Action<UpgradeType> Upgraded;

		public int UpgradeStage(UpgradeType upgradeType) => _playerData.Resources.GetUpgradeStage(upgradeType);

	    public int CalculateUpgradedGoldAmount(int baseGold) =>
	                (int)(baseGold * UpgradeStageValue(UpgradeType.IncreaseRewards));

	    public void AddUpgradeStage(UpgradeType upgradeType)
	    {
	        int upgradeStage = UpgradeStage(upgradeType);
	        upgradeStage++;
	        _playerData.Resources.SetUpgradeStage(upgradeType, upgradeStage);
	        _playerData.Save();
	        Upgraded?.Invoke(upgradeType);
	    }

	    public float UpgradeStageValue(UpgradeType upgradeType)
	    {
	        int upgradeStage = _playerData.Resources.GetUpgradeStage(upgradeType);
	        UpgradeSettingsData upgrade = _upgradeSettings.Upgrades.FirstOrDefault(upgrade => upgrade.Type == upgradeType);

	        return (upgradeStage == 0) ?
	                upgrade.DefaultValue :
	                upgrade.StageValues[upgradeStage - 1];
	    }
	}
}
