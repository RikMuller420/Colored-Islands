using System.Collections.Generic;

public class ScoreCalculator
{
    private int _anyTryScore = 1500;
    private int _scorePerUnit = 100;
    private int _scorePerSavedSecond = 200;
    private int _scorePerSavedMove = 200;

    private LevelProgressTracker _progressTracker;

    public ScoreCalculator(LevelProgressTracker progressTracker)
    {
        _progressTracker = progressTracker;
    }

    public int CalculateScore(float levelTime, int levelMoves, IReadOnlyCollection<Island> islands)
    {
        int score = 0;
        score += _anyTryScore;

        foreach (Island island in islands)
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
