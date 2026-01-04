using System;
using System.Collections;
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
        if (targetIsland.FreePointsCount == 0)
        {
            return;
        }

        IEnumerable<Unit> islandUnits = homeIsland.GetUnits(unitsPaint);
        IReadOnlyCollection<Unit> movedUnits = islandUnits
                        .OrderBy(unit => Vector3.SqrMagnitude(unit.transform.position - targetIsland.transform.position))
                        .Take(targetIsland.FreePointsCount)
                        .ToList();

        IEnumerable<Unit> homeUnits = targetIsland.Points
                                    .Where(point => !point.IsFree)
                                    .Select(point => point.OccupiedUnit)
                                    .ToList()
                                    .AsReadOnly();



        foreach (Unit unit in movedUnits)
        {
            MoveUnit(unit, targetIsland);
        }

        OptimizeUnitsPosition(targetIsland);
        UnitsMoveInfo unitsMoveInfo = new UnitsMoveInfo(homeIsland, targetIsland, unitsPaint, movedUnits);

        foreach (Unit homeUnit in homeUnits)
        {
            homeUnit.LookToTarget(homeIsland.transform, unitsMoveInfo);
        }

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
