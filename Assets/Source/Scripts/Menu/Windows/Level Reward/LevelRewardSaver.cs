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
            int newGoldAmount = _playerData.GoldAmount + goldReward;
            _playerData.SetGoldAmount(newGoldAmount);
        }

        if (reward.RouletteSpinAmount > 0)
        {
            int spinCount = reward.RouletteSpinAmount * multiplier;
            int newSpinAmount = _playerData.AviableSpinCount + spinCount;
            _playerData.SetSpinCount(newSpinAmount);
            MetricSaver.GetRouleteSpin(spinCount);
        }

        if (reward.BoostAmount > 0)
        {
            int boostAmount = _playerData.GetBoostAmount(reward.BoostType) + reward.BoostAmount * multiplier;
            _playerData.SetBoostAmount(reward.BoostType, boostAmount);
        }

        _playerData.MarkLevelRewardReceived(reward.LevelId);
        _playerData.Save();
    }
}
