public class ScoreCalculator
{
    private int _anyTryScore = 1500;
    private int _scorePerUnit = 100;
    private int _scorePerSavedSecond = 200;
    private int _scorePerSavedMove = 200;

    private LevelProgressTracker _progressTracker;
    private LevelObjectsHolder _levelDataHolder;

    public ScoreCalculator(LevelProgressTracker progressTracker, LevelObjectsHolder levelDataHolder)
    {
        _progressTracker = progressTracker;
        _levelDataHolder = levelDataHolder;
    }

    public int CalculateScore(float levelTime, int levelMoves)
    {
        int score = 0;
        score += _anyTryScore;

        foreach (Island island in _levelDataHolder.Islands)
        {
            if (island.IsDone)
            {
                score += _scorePerUnit * island.Points.Count;
            }
        }

        if (_progressTracker.IsLevelFinished)
        {
            if (levelTime < _progressTracker.LevelData.ExtraStarTimeLimit)
            {
                score += (int)(_progressTracker.LevelData.ExtraStarTimeLimit - levelTime) * _scorePerSavedSecond;
            }

            if (levelMoves < _progressTracker.LevelData.LevelMoveLimit)
            {
                score += (int)(_progressTracker.LevelData.LevelMoveLimit - levelMoves) * _scorePerSavedMove;
            }
        }

        return score;
    }
}
