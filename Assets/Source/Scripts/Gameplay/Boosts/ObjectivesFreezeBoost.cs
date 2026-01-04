using System;

public class ObjectivesFreezeBoost : Boost
{
    private LevelProgressTracker _levelProgressTracker;
    private LevelLoader _levelLoader;

    private int _usedMoves = 0;
    private int _maxMoves = 7;
    private bool _isBoostApplying = false;

    public event Action BoostStopApplyed;

    public ObjectivesFreezeBoost(LevelProgressTracker levelProgressTracker, UnitMover unitMover,
                                LevelLoader levelLoader, BoostAmountProvider boostAmountProvider) :
                                base(boostAmountProvider)       
    {
        _levelProgressTracker = levelProgressTracker;
        _levelLoader = levelLoader;

        unitMover.UnitsMoved += OnUnitMoved;
        levelLoader.LevelChanged += OnLevelChanged;
    }

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

    private void OnLevelChanged()
    {
        StopBoostApplying();
    }

    private void StopBoostApplying()
    {
        if (_isBoostApplying)
        {
            _isBoostApplying = false;
            BoostStopApplyed?.Invoke();
        }
    }
}
