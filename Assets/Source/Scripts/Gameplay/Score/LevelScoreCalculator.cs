using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Integration.Metrics;

namespace SlimeGround.Gameplay.Score
{
	public class LevelScoreCalculator
	{
	    private const int AnyTryScore = 1500;
	    private const int ScorePerUnit = 100;
	    private const int ScorePerSavedSecond = 200;
	    private const int ScorePerSavedMove = 200;

		private readonly ILevelData _currentLevelData;

		public LevelScoreCalculator(ILevelData currentLevelData)
	    {
	        _currentLevelData = currentLevelData;
	    }

		public int CalculateScore(float levelTime, int levelMoves)
	    {
	        int score = AnyTryScore;

	        foreach (Island island in _currentLevelData.Islands)
	        {
	            score += ScorePerUnit * island.Points.Count;
	        }

	        if (levelTime < _currentLevelData.ExtraScoreTime)
	        {
	            score += (int)(_currentLevelData.ExtraScoreTime - levelTime) * ScorePerSavedSecond;
	        }

	        if (levelMoves < _currentLevelData.ExtraStarMoveCount)
	        {
	            score += (int)(_currentLevelData.ExtraStarMoveCount - levelMoves) * ScorePerSavedMove;
	        }
	        else
	        {
	            MetricSaver.TrackMoveLimitFailed();
	        }

	        return score;
	    }
	}
}
