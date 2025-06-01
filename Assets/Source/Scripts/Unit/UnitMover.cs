using System;
using System.Collections.Generic;

public class UnitMover
{
    public event Action<UnitsMoveInfo> UnitsMoved;

    public void MoveAllPossibleUnits(BaseIsland homeIsland, Paint unitsPaint, BaseIsland targetIsland)
    {
        List<Unit> units = new List<Unit>();

        foreach (Unit unit in homeIsland.GetUnits(unitsPaint))
        {
            if (targetIsland.FreePointsCount == 0)
            {
                break;
            }

            MoveUnit(unit, targetIsland);
            units.Add(unit);
        }

        UnitsMoveInfo unitsMoveInfo = new UnitsMoveInfo(homeIsland, targetIsland, unitsPaint, units);
        UnitsMoved?.Invoke(unitsMoveInfo);
    }

    public void MoveUnit(Unit unit, BaseIsland targetIsland)
    {
        unit.Island.RemoveUnit(unit);
        targetIsland.AddUnit(unit, out IslandPoint placementPoint);
        unit.SetIsland(targetIsland);
        unit.transform.position = placementPoint.Point.position;
    }
}
