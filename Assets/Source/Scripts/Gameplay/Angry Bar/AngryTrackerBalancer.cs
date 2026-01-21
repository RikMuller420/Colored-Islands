using SlimeGround.Gameplay.Levels;
using UnityEngine;

namespace SlimeGround.Gameplay.AngryBar
{
	public class AngryTrackerBalancer
	{
	    private LevelProgressTracker _progressTracker;
	    private LevelChangeEventTracker _levelChangeEventTracker;

	    private float _angryValueLimit = 0.3f;
	    private bool _isCurrentLevelFinished = false;

	    private float _step = 0.2f;
	    private float _minValue = 0.2f;
	    private float _maxValue = 1.8f;
	    private float _defaultValue = 1f;

	    private int _winStreak = 0;
	    private int _loseStreak = 0;

	    public float Value { get; private set; } = 1f;

	    public AngryTrackerBalancer(LevelProgressTracker progressTracker, LevelChangeEventTracker levelChangeEventTracker)
	    {
	        _progressTracker = progressTracker;
	        _levelChangeEventTracker = levelChangeEventTracker;

	        _progressTracker.LevelFinished += RecordWin;
	        _levelChangeEventTracker.LevelStartChanging += OnLevelStartChanging;
	    }

	    private void OnLevelStartChanging()
	    {
	        if (_isCurrentLevelFinished == false && _progressTracker.AngryValue > _angryValueLimit)
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
	            Value = 1;
	        }

	        _loseStreak++;

	        Value = _defaultValue - (_loseStreak * _step);
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

	        Value = _defaultValue + (_winStreak * _step);
	        ClampValue();
	    }

	    private void ClampValue() => Value = Mathf.Clamp(Value, _minValue, _maxValue);
	}
}
