public class BufferIslandBoost : Boost
{
    private BuferIslandsHolder _buferIslandsHolder;
    private UnitMover _unitMover;

    public BufferIslandBoost(BuferIslandsHolder buferIslandsHolder, UnitMover unitMover)
    {
        _buferIslandsHolder = buferIslandsHolder;
        _unitMover = unitMover;
    }

    public override void TryApplyBoost()
    {
        BaseIsland oldIsland = _buferIslandsHolder.CurrentIsland;
        int newSize = oldIsland.Points.Count + 1;

        _buferIslandsHolder.DeactivateCurrentIsland();
        _buferIslandsHolder.LoadIsland(newSize);
        BaseIsland newIsland = _buferIslandsHolder.CurrentIsland;

        foreach (PlacementPoint point in oldIsland.Points)
        {
            if (point.IsFree)
            {
                continue;
            }

            _unitMover.MoveUnit(point.OccupiedUnit, newIsland);
        }

        InvokeBoostApplyedEvent();
    }
}
