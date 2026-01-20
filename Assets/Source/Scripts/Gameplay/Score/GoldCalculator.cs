using System.Linq;

public class GoldCalculator
{
    private int _goldPerNewStar = 50;
    private int _goldPerReEarnedStar = 5;

    private IPlayerData _playerData;
    private IUpgradesData _upgradesData;
    private ILevelData _currentLevelData;

    public GoldCalculator(IPlayerData playerData, IUpgradesData upgradesData,
                          ILevelData currentLevelData)
    {
        _playerData = playerData;
        _upgradesData = upgradesData;
        _currentLevelData = currentLevelData;
    }

    public int CalculateLevelGold(bool isAngryTaskDone, bool isMoveTaskDone)
    {
        LevelProgress savedProgress = _playerData.Levels
                                    .FirstOrDefault(level => level.Id == _currentLevelData.LevelId);
        int gold = 0;
        gold += savedProgress.IsDone ? _goldPerReEarnedStar : _goldPerNewStar;

        if (isAngryTaskDone)
        {
            gold += savedProgress.IsAngryStarEarned ? _goldPerReEarnedStar : _goldPerNewStar;
        }

        if (isMoveTaskDone)
        {
            gold += savedProgress.IsMovesStarEarned ? _goldPerReEarnedStar : _goldPerNewStar;
        }

        return _upgradesData.CalculateUpgradedGoldAmount(gold);
    }
}
