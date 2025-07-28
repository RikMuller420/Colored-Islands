using System.Linq;

public class GoldCalculator
{
    private int _goldPerNewStar = 50;
    private int _goldPerReEarnedStar = 5;

    private LevelProgressTracker _progressTracker;
    private GameProgressStorage _progressStorage;
    private UpgradesProvider _upgradesProvider;

    public GoldCalculator(LevelProgressTracker progressTracker, GameProgressStorage gameProgressStorage,
                        UpgradesProvider upgradesProvider)
    {
        _progressTracker = progressTracker;
        _progressStorage = gameProgressStorage;
        _upgradesProvider = upgradesProvider;
    }

    public int CalculateGold()
    {
        if (_progressTracker.IsLevelFinished == false)
        {
            return 0;
        }

        LevelProgress savedProgress = _progressStorage.Levels
                                    .FirstOrDefault(level => level.Id == _progressTracker.LevelData.Id);
        int gold = 0;
        gold += savedProgress.IsDone ? _goldPerReEarnedStar : _goldPerNewStar;

        if (_progressTracker.IsAngryTaskDone)
        {
            gold += savedProgress.IsAngryTaskDone ? _goldPerReEarnedStar : _goldPerNewStar;
        }

        if (_progressTracker.IsMoveTaskDone)
        {
            gold += savedProgress.IsMoveTaskDone ? _goldPerReEarnedStar : _goldPerNewStar;
        }

        return _upgradesProvider.CalculateUpgradedGoldAmount(gold);
    }
}
