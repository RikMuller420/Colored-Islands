using SlimeGround.Gameplay.Levels;
using UnityEngine;

namespace SlimeGround.Gameplay.AngryBar
{
	public class AngerTrackerBalancer
	{
		private const float AngryValueLimit = 0.3f;
		private const float Step = 0.2f;
		private const float MinValue = 0.2f;
		private const float MaxValue = 1.8f;
		private const float DefaultValue = 1f;

		private readonly LevelProgressTracker _progressTracker;
		private readonly LevelChangeEventTracker _levelChangeEventTracker;

	    private int _winStreak = 0;
	    private int _loseStreak = 0;
		private bool _isCurrentLevelFinished = false;

	    public AngerTrackerBalancer(LevelProgressTracker progressTracker, LevelChangeEventTracker levelChangeEventTracker)
	    {
	        _progressTracker = progressTracker;
	        _levelChangeEventTracker = levelChangeEventTracker;

	        _progressTracker.LevelFinished += RecordWin;
	        _levelChangeEventTracker.LevelStartChanging += OnLevelStartChanging;
	    }

		public float Value { get; private set; } = DefaultValue;

		public void Dispose()
		{
			_progressTracker.LevelFinished -= RecordWin;
			_levelChangeEventTracker.LevelStartChanging -= OnLevelStartChanging;
		}

		private void OnLevelStartChanging()
	    {
	        if (_isCurrentLevelFinished == false && _progressTracker.AngryValue > AngryValueLimit)
	        {
	            RecordLose();
	        }

	        _isCurrentLevelFinished = false;
	    }

	    private void RecordLose()
	    {
	        if (_winStreak != 0)
	        {
	            _winStreak = 0;
	            Value = DefaultValue;
	        }

	        _loseStreak++;

	        Value = DefaultValue - (_loseStreak * Step);
	        ClampValue();
	    }

	    private void RecordWin(ILevelData _)
	    {
	        if (_loseStreak != 0)
	        {
	            _loseStreak = 0;
	            Value = 1;
	        }

	        _isCurrentLevelFinished = true;
	        _loseStreak = 0;
	        _winStreak++;

	        Value = DefaultValue + (_winStreak * Step);
	        ClampValue();
	    }

	    private void ClampValue() => Value = Mathf.Clamp(Value, MinValue, MaxValue);
	}
}
