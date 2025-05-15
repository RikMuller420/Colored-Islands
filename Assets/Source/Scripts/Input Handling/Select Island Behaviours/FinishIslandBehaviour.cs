using System.Collections.Generic;
using System.Linq;

public class FinishIslandBehaviour : SelectIslandBehaviour
{
    private SelectHandler _selectHandler;
    private UnitMover _unitMover;
    private LevelObjectsHolder _levelDataHolder;
    private BuferIslandsHolder _buferIslands;

    public FinishIslandBehaviour(SelectHandler selectHandler, LevelObjectsHolder levelDataHolder,
                                BuferIslandsHolder buferIslands, UnitMover unitMover)
    {
        _selectHandler = selectHandler;
        _levelDataHolder = levelDataHolder;
        _unitMover = unitMover;
        _buferIslands = buferIslands;
    }

    public void SelectIsland(BaseIsland baseIsland)
    {
        if (baseIsland is Island island == false)
        {
            return;
        }

        IReadOnlyCollection<PlacementPoint> freePoints = island.Points
            .Where(point => point.IsFree)
            .ToList()
            .AsReadOnly();

        IReadOnlyCollection<PlacementPoint> wrongUnitPoints = island.Points
            .Where(point => point.IsFree == false && point.OccupiedUnit.Paint != island.Paint)
            .ToList()
            .AsReadOnly();

        foreach (PlacementPoint point in freePoints)
        {
            MoveAnySuitableUnit(island);
        }

        foreach (PlacementPoint point in wrongUnitPoints)
        {
            BaseIsland freeIsland = FindFreeIsland();
            _unitMover.MoveUnit(point.OccupiedUnit, freeIsland);
            MoveAnySuitableUnit(island);
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
        PlacementPoint point = island.Points.FirstOrDefault(point =>
                            point.IsFree == false && point.OccupiedUnit.Paint == unitPaint);

        if (point != null)
        {
            unit = point.OccupiedUnit;

            return true;
        }

        return false;
    }
}
