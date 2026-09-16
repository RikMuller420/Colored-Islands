using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.LevelRewards;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Windows.GameShop.Upgrades;

namespace SlimeGround.Menu.Windows.LevelReward
{
	public class LevelRewardSaver
	{
	    private PlayerDataProvider _playerData;
	    private UpgradesProvider _upgradesProvider;

	    public LevelRewardSaver(PlayerDataProvider playerData, UpgradesProvider upgradesProvider)
	    {
	        _playerData = playerData;
	        _upgradesProvider = upgradesProvider;
	    }

	    public void AddReward(LevelRewardData reward, int multiplier = 1)
	    {
	        if (reward.GoldAmount > 0)
	        {
	            int goldReward = _upgradesProvider.CalculateUpgradedGoldAmount(reward.GoldAmount);
	            int newGoldAmount = _playerData.Resources.GoldAmount + goldReward;
	            _playerData.Resources.SetGoldAmount(newGoldAmount);
	        }

	        if (reward.RouletteSpinAmount > 0)
	        {
	            int spinCount = reward.RouletteSpinAmount * multiplier;
	            int newSpinAmount = _playerData.Resources.AviableSpinCount + spinCount;
	            _playerData.Resources.SetSpinCount(newSpinAmount);
	        }

	        if (reward.BoostAmount > 0)
	        {
	            int boostAmount = _playerData.Resources.GetBoostAmount(reward.BoostType) + (reward.BoostAmount * multiplier);
	            _playerData.Resources.SetBoostAmount(reward.BoostType, boostAmount);
	        }

	        _playerData.Progress.MarkLevelRewardReceived(reward.LevelId);
	        _playerData.Save();
	    }
	}
}
