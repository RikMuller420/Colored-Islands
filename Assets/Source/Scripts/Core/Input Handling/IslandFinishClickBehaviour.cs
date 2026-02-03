using System;
using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Core.InputHandling
{
	public class IslandFinishClickBehaviour : ClickBehaviour, IIslandFinishEvent
	{
	    private UnitMover _unitMover;
	    private ILevelData _currentLevelData;
	    private BuferIslandsHolder _buferIslands;

	    public IslandFinishClickBehaviour(ILevelData currentLevelData, BuferIslandsHolder buferIslands,
	                                      UnitMover unitMover, LayerMask layerMask) : base(layerMask)
	    {
	        _currentLevelData = currentLevelData;
	        _unitMover = unitMover;
	        _buferIslands = buferIslands;
	    }

		public event Action<Island> IslandFinished;

		public override void HandleClick(RaycastHit hit)
	    {
	        if (hit.collider.TryGetComponent(out Island island))
	        {
	            FinishIsland(island);
	            IslandFinished?.Invoke(island);
	        }
	    }

	    public override void ResetBehaviour() { return; }

	    private void FinishIsland(Island island)
	    {
	        IReadOnlyCollection<IslandPoint> freePoints = island.Points
	            .Where(point => point.IsFree)
	            .ToList()
	            .AsReadOnly();

	        IReadOnlyCollection<IslandPoint> wrongUnitPoints = island.Points
	            .Where(point => point.IsFree == false && point.OccupiedUnit.Slot != island.RequredUnitSlot)
	            .ToList()
	            .AsReadOnly();

	        List<BaseIsland> usedIslands = new List<BaseIsland>() { island };

	        foreach (IslandPoint point in freePoints)
	        {
	            MoveAnySuitableUnit(island);
	        }

	        foreach (IslandPoint point in wrongUnitPoints)
	        {
	            BaseIsland freeIsland = FindFreeIsland();
	            _unitMover.MoveUnit(point.OccupiedUnit, freeIsland);
	            MoveAnySuitableUnit(island);

	            if (usedIslands.Contains(freeIsland) == false)
	            {
	                usedIslands.Add(freeIsland);
	            }
	        }

	        foreach (BaseIsland usedIsland in usedIslands)
	        {
	            _unitMover.OptimizeUnitsPosition(usedIsland);
	        }
	    }

	    private void MoveAnySuitableUnit(Island targetIsland)
	    {
	        Unit suitableUnit = FindSutableUnit(targetIsland);
	        _unitMover.MoveUnit(suitableUnit, targetIsland);
	    }

	    private BaseIsland FindFreeIsland()
	    {
	        foreach (Island island in _currentLevelData.Islands)
	        {
	            if (island.Points.Any(point => point.IsFree))
	            {
	                return island;
	            }
	        }

	        return _buferIslands.CurrentIsland;
	    }

	    private Unit FindSutableUnit(Island homeIsland)
	    {
	        Unit unit;

	        foreach (Island island in _currentLevelData.Islands)
	        {
	            if (island == homeIsland || island.IsDone)
	            {
	                continue;
	            }

	            if (TryFindUnit(island, homeIsland.RequredUnitSlot, out unit))
	            {
	                return unit;
	            }
	        }

	        if (TryFindUnit(_buferIslands.CurrentIsland, homeIsland.RequredUnitSlot, out unit))
	        {
	            return unit;
	        }

	        return null;
	    }

	    private bool TryFindUnit(BaseIsland island, UnitSlotType unitSlot, out Unit unit)
	    {
	        unit = null;
	        IslandPoint point = island.Points.FirstOrDefault(point =>
	                            point.IsFree == false && point.OccupiedUnit.Slot == unitSlot);

	        if (point != null)
	        {
	            unit = point.OccupiedUnit;

	            return true;
	        }

	        return false;
	    }
	}
}
