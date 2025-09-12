public class LevelRewardSaver
{
    private GameProgressStorage _progressStorage;
    private UpgradesProvider _upgradesProvider;

    public LevelRewardSaver(GameProgressStorage progressStorage, UpgradesProvider upgradesProvider)
    {
        _progressStorage = progressStorage;
        _upgradesProvider = upgradesProvider;
    }

    public void AddReward(LevelRewardData reward, int multiplier = 1)
    {
        if (reward.GoldAmount > 0)
        {
            int goldReward = _upgradesProvider.CalculateUpgradedGoldAmount(reward.GoldAmount);
            int newGoldAmount = _progressStorage.GoldAmount + goldReward;
            _progressStorage.SetGoldAmount(newGoldAmount);
        }

        if (reward.RouletteSpinAmount > 0)
        {
            int newSpinAmount = _progressStorage.AviableSpinCount + reward.RouletteSpinAmount * multiplier;
            _progressStorage.SetSpinCount(newSpinAmount);
        }

        if (reward.BoostAmount > 0)
        {
            int boostAmount = _progressStorage.GetBoostAmount(reward.BoostType) + reward.BoostAmount * multiplier;
            _progressStorage.SetBoostAmount(reward.BoostType, boostAmount);
        }

        _progressStorage.MarkLevelRewardReceived(reward.LevelId);
        _progressStorage.Save();
    }
}
