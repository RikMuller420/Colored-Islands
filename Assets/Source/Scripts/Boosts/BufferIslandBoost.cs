[System.Serializable]
public class BufferIslandBoost : Boost
{
    private const int ExtraSize = 2;

    private BuferIslandsHolder _buferIslandsHolder;
    private UnitMover _unitMover;

    public BufferIslandBoost(BuferIslandsHolder buferIslandsHolder, UnitMover unitMover,
                            BoostAmountProvider boostAmountProvider) : base(boostAmountProvider)
    {
        _buferIslandsHolder = buferIslandsHolder;
        _unitMover = unitMover;
    }

    public override BoostType Type => BoostType.GrowBuferIsland;

    public override void TryApplyBoost()
    {
        BaseIsland oldIsland = _buferIslandsHolder.CurrentIsland;
        int newSize = oldIsland.Points.Count + ExtraSize;

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

        SpendBoost(Type);
    }
}
