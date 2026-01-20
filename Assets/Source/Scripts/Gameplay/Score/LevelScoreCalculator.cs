public class LevelScoreCalculator
{
    private int _anyTryScore = 1500;
    private int _scorePerUnit = 100;
    private int _scorePerSavedSecond = 200;
    private int _scorePerSavedMove = 200;

    private ILevelData _currentLevelData;

    public LevelScoreCalculator(ILevelData currentLevelData)
    {
        _currentLevelData = currentLevelData;
    }

    public int CalculateScore(float levelTime, int levelMoves)
    {
        int score = _anyTryScore;

        foreach (Island island in _currentLevelData.Islands)
        {
            score += _scorePerUnit * island.Points.Count;
        }

        if (levelTime < _currentLevelData.ExtraScoreTime)
        {
            score += (int)(_currentLevelData.ExtraScoreTime - levelTime) * _scorePerSavedSecond;
        }

        if (levelMoves < _currentLevelData.ExtraStarMoveCount)
        {
            score += (int)(_currentLevelData.ExtraStarMoveCount - levelMoves) * _scorePerSavedMove;
        }
        else
        {
            MetricSaver.TrackMoveLimitFailed();
        }

        return score;
    }
}
