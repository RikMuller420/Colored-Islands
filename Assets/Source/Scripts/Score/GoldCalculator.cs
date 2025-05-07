public class GoldCalculator
{
    private int _goldPerStar = 50;

    private LevelProgressTracker _progressTracker;

    public GoldCalculator(LevelProgressTracker progressTracker)
    {
        _progressTracker = progressTracker;
    }

    public int CalculateGold()
    {
        int gold = 0;

        if (_progressTracker.IsLevelFinished)
        {
            gold += _goldPerStar;

            if (_progressTracker.IsTimeTaskDone)
            {
                gold += _goldPerStar;
            }

            if (_progressTracker.IsMoveTaskDone)
            {
                gold += _goldPerStar;
            }
        }

        return gold;
    }
}
