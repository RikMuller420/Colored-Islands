using System;
using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data;
using SlimeGround.Gameplay.Islands;
using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	public class UnitMover : IUnitMovedEvent
	{
	    private IslandPaintDistributor _islandSlotDistributor;
	    private Transform _unitsLookAtTarget;

	    public UnitMover(Transform unitsLookAtTarget)
	    {
	        _unitsLookAtTarget = unitsLookAtTarget;
	        _islandSlotDistributor = new IslandPaintDistributor();
	    }

		public event Action<UnitsMoveInfo> UnitsMoved;

		public void MoveAllPossibleUnits(BaseIsland homeIsland, UnitSlotType unitSlot, BaseIsland targetIsland)
	    {
	        if (targetIsland.FreePointsCount == 0)
	        {
	            return;
	        }

	        IEnumerable<Unit> islandUnits = homeIsland.GetUnits(unitSlot);
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
	        UnitsMoveInfo unitsMoveInfo = new UnitsMoveInfo(homeIsland, targetIsland, unitSlot, movedUnits);

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
	        Dictionary<IslandPoint, UnitSlotType> requredSlotDistribution = _islandSlotDistributor.CalculateOptimalSlotDistribution(island);
	        Dictionary<IslandPoint, UnitSlotType> pointToFill = new Dictionary<IslandPoint, UnitSlotType>();
	        List<Unit> unitsToMove = new List<Unit>();

	        foreach (IslandPoint point in island.Points)
	        {
	            if (requredSlotDistribution.ContainsKey(point) == false && point.IsFree == false)
	            {
	                unitsToMove.Add(point.OccupiedUnit);
	                point.RemoveUnit();
	            }
	        }

	        foreach (var requredPointSlot in requredSlotDistribution)
	        {
	            IslandPoint point = requredPointSlot.Key;
	            UnitSlotType slot = requredPointSlot.Value;

	            if (point.IsFree)
	            {
	                pointToFill.Add(point, slot);
	            }
	            else if (point.OccupiedUnit.Slot != slot)
	            {
	                pointToFill.Add(point, slot);
	                unitsToMove.Add(point.OccupiedUnit);
	            }
	        }

	        foreach (var requredPointSlot in pointToFill)
	        {
	            IslandPoint point = requredPointSlot.Key;
	            UnitSlotType slot = requredPointSlot.Value;

	            Unit unit = unitsToMove.FirstOrDefault(unit => unit.Slot == slot);

	            point.SetUnit(unit);
	            UnitMoveTask unitMoveTask = new UnitMoveTask(unit, point, _unitsLookAtTarget);
	            unitsToMove.Remove(unit);
	        }
	    }
	}
}
