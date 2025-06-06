using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMover
{
    private IslandPaintDistributor _islandPaintDistributor;
    private Transform _unitsLookAtTarget;

    public event Action<UnitsMoveInfo> UnitsMoved;

    public UnitMover(Transform unitsLookAtTarget)
    {
        _unitsLookAtTarget = unitsLookAtTarget;
        _islandPaintDistributor = new IslandPaintDistributor();
    }

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

        OptimizeUnitsPosition(targetIsland);
        UnitsMoveInfo unitsMoveInfo = new UnitsMoveInfo(homeIsland, targetIsland, unitsPaint, units);
        UnitsMoved?.Invoke(unitsMoveInfo);
    }

    public void MoveUnit(Unit unit, BaseIsland targetIsland)
    {
        unit.Island.RemoveUnit(unit);
        targetIsland.AddUnitToFreePoint(unit, out IslandPoint targetPoint);
        unit.SetIsland(targetIsland);

        UnitMoveTask unitMoveTask = new UnitMoveTask(unit, targetPoint, _unitsLookAtTarget);
    }

    public void OptimizeUnitsPosition(BaseIsland island)
    {
        Dictionary<IslandPoint, Paint> requredPaintsDistribution = _islandPaintDistributor.CalculateOptimalPaintDistribution(island);
        Dictionary<IslandPoint, Paint> pointToFill = new Dictionary<IslandPoint, Paint>();
        List<Unit> unitsToMove = new List<Unit>();

        foreach (IslandPoint point in island.Points)
        {
            if (requredPaintsDistribution.ContainsKey(point) == false && point.IsFree == false)
            {
                unitsToMove.Add(point.OccupiedUnit);
                point.RemoveUnit();
            }
        }

        foreach (var requredPointPaint in requredPaintsDistribution)
        {
            IslandPoint point = requredPointPaint.Key;
            Paint paint = requredPointPaint.Value;

            if (point.IsFree)
            {
                pointToFill.Add(point, paint);
            }
            else if(point.OccupiedUnit.Paint != paint)
            {
                pointToFill.Add(point, paint);
                unitsToMove.Add(point.OccupiedUnit);
            }
        }

        foreach (var requredPointPaint in pointToFill)
        {
            IslandPoint point = requredPointPaint.Key;
            Paint paint = requredPointPaint.Value;

            Unit unit = unitsToMove.FirstOrDefault(unit => unit.Paint == paint);

            point.SetUnit(unit);
            UnitMoveTask unitMoveTask = new UnitMoveTask(unit, point, _unitsLookAtTarget);
            unitsToMove.Remove(unit);
        }
    }
}
