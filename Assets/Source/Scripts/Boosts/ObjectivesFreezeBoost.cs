public class ObjectivesFreezeBoost 
{
    private LevelProgressTracker _levelProgressTracker;

    private int _usedMoves = 0;
    private int _maxMoves = 7;

    public ObjectivesFreezeBoost(LevelProgressTracker levelProgressTracker, UnitMover unitMover)
    {
        _levelProgressTracker = levelProgressTracker;
        unitMover.UnitsMoved += OnUnitMoved;
    }

    public void FreezeObjectives()
    {
        _usedMoves = 0;
        _levelProgressTracker.PauseTracking();
    }

    private void OnUnitMoved()
    {
        _usedMoves++;

        if (_usedMoves == _maxMoves)
        {
            _levelProgressTracker.ContinueTracking();
        }
    }
}
