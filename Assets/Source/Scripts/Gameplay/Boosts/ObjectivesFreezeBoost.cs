using System;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Gameplay.Units;

namespace SlimeGround.Gameplay.Boosts
{
	public class ObjectivesFreezeBoost : Boost, IBoostStopApplyedEvent
	{
	    private LevelProgressTracker _levelProgressTracker;
	    private LevelChangeEventTracker _levelChangeEventTracker;

	    private int _usedMoves = 0;
	    private int _maxMoves = 7;
	    private bool _isBoostApplying = false;

	    public ObjectivesFreezeBoost(LevelProgressTracker levelProgressTracker, UnitMover unitMover,
	                                LevelChangeEventTracker levelChangeEventTracker,
									BoostAmountProvider boostAmountProvider) : base(boostAmountProvider)       
	    {
	        _levelProgressTracker = levelProgressTracker;
	        _levelChangeEventTracker = levelChangeEventTracker;

	        unitMover.UnitsMoved += OnUnitMoved;
	        levelChangeEventTracker.LevelChanged += OnLevelChanged;
	    }

		public event Action StopApplyed;

		public override BoostType Type => BoostType.FreezeObjectives;

	    public override void TryApplyBoost()
	    {
	        _usedMoves = 0;
	        _levelProgressTracker.PauseTracking();
	        _isBoostApplying = true;
	        SpendBoost(Type);
	    }

	    private void OnUnitMoved(UnitsMoveInfo _)
	    {
	        if (_isBoostApplying == false)
	        {
	            return;
	        }

	        _usedMoves++;

	        if (_usedMoves == _maxMoves)
	        {
	            _levelProgressTracker.ContinueTracking();
	            StopBoostApplying();
	        }
	    }

	    private void OnLevelChanged(ILevelData _)
	    {
	        StopBoostApplying();
	    }

	    private void StopBoostApplying()
	    {
	        if (_isBoostApplying)
	        {
	            _isBoostApplying = false;
	            StopApplyed?.Invoke();
	        }
	    }
	}
}
