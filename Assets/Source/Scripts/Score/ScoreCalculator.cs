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
            if (levelTime < _levelDataHolder.ExtraScoreTime)
            {
                score += (int)(_levelDataHolder.ExtraScoreTime - levelTime) * _scorePerSavedSecond;
            }

            if (levelMoves < _levelDataHolder.ExtraStarMoveCount)
            {
                score += (int)(_levelDataHolder.ExtraStarMoveCount - levelMoves) * _scorePerSavedMove;
            }
            else
            {
                MetricSaver.TrackMoveLimitFailed();
            }
        }

        return score;
    }
}
