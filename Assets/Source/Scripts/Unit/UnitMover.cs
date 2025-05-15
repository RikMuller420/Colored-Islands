using System;

public class UnitMover
{
    public event Action UnitsMoved;

    public void MoveAllPossibleUnits(BaseIsland homeIsland, Paint unitsPaint, BaseIsland targetIsland)
    {
        foreach (Unit unit in homeIsland.GetUnits(unitsPaint))
        {
            if (targetIsland.FreePointsCount == 0)
            {
                break;
            }

            MoveUnit(unit, targetIsland);
        }

        UnitsMoved?.Invoke();
    }

    public void MoveUnit(Unit unit, BaseIsland targetIsland)
    {
        unit.Island.RemoveUnit(unit);
        targetIsland.AddUnit(unit, out PlacementPoint placementPoint);
        unit.SetIsland(targetIsland);
        unit.transform.position = placementPoint.Point.position;
    }
}
