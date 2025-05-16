public class ObjectivesFreezeBoost : Boost
{
    private LevelProgressTracker _levelProgressTracker;
    private LevelLoader _levelLoader;

    private int _usedMoves = 0;
    private int _maxMoves = 7;
    private bool _isBoostApplying = false;

    public ObjectivesFreezeBoost(LevelProgressTracker levelProgressTracker, UnitMover unitMover,
                                LevelLoader levelLoader)
    {
        _levelProgressTracker = levelProgressTracker;
        _levelLoader = levelLoader;

        unitMover.UnitsMoved += OnUnitMoved;
        levelLoader.LevelChanged += OnLevelChanged;
    }

    public override void TryApplyBoost()
    {
        _usedMoves = 0;
        _levelProgressTracker.PauseTracking();
        _isBoostApplying = true;
        InvokeBoostApplyedEvent();
    }

    private void OnUnitMoved()
    {
        if (_isBoostApplying == false)
        {
            return;
        }

        _usedMoves++;

        if (_usedMoves == _maxMoves)
        {
            _levelProgressTracker.ContinueTracking();
            _isBoostApplying = false;
        }
    }

    private void OnLevelChanged()
    {
        if (_isBoostApplying)
        {
            _isBoostApplying = false;
        }
    }
}
