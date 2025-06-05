using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandFinishBehaviour : ClickBehaviour
{
    private UnitMover _unitMover;
    private LevelObjectsHolder _levelDataHolder;
    private BuferIslandsHolder _buferIslands;

    public event Action IslandFinished;

    public IslandFinishBehaviour(LevelObjectsHolder levelDataHolder, BuferIslandsHolder buferIslands,
                                        UnitMover unitMover, LayerMask layerMask) : base(layerMask)
    {
        _levelDataHolder = levelDataHolder;
        _unitMover = unitMover;
        _buferIslands = buferIslands;
    }

    public override void HandleClick(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out Island island))
        {
            FinishIsland(island);
            IslandFinished?.Invoke();
        }
    }

    private void FinishIsland(Island island)
    {
        IReadOnlyCollection<IslandPoint> freePoints = island.Points
            .Where(point => point.IsFree)
            .ToList()
            .AsReadOnly();

        IReadOnlyCollection<IslandPoint> wrongUnitPoints = island.Points
            .Where(point => point.IsFree == false && point.OccupiedUnit.Paint != island.Paint)
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
        foreach (Island island in _levelDataHolder.Islands)
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

        foreach (Island island in _levelDataHolder.Islands)
        {
            if (island == homeIsland || island.IsDone)
            {
                continue;
            }

            if (TryFindUnit(island, homeIsland.Paint, out unit))
            {
                return unit;
            }
        }

        if (TryFindUnit(_buferIslands.CurrentIsland, homeIsland.Paint, out unit))
        {
            return unit;
        }

        return null;
    }

    private bool TryFindUnit(BaseIsland island, Paint unitPaint, out Unit unit)
    {
        unit = null;
        IslandPoint point = island.Points.FirstOrDefault(point =>
                            point.IsFree == false && point.OccupiedUnit.Paint == unitPaint);

        if (point != null)
        {
            unit = point.OccupiedUnit;

            return true;
        }

        return false;
    }
}
