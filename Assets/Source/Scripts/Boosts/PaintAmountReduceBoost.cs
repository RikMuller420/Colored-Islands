using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class PaintAmountReduceBoost : Boost
{
    private LevelObjectsHolder _levelDataHolder;
    private BuferIslandsHolder _buferIslands;
    private PaintMaterials _paintMaterials;

    private int _bestNewColorIndex = 2;

    public PaintAmountReduceBoost(LevelObjectsHolder levelDataHolder, BuferIslandsHolder buferIslands,
                                    PaintMaterials paintMaterials)
    {
        _levelDataHolder = levelDataHolder;
        _buferIslands = buferIslands;
        _paintMaterials = paintMaterials;
    }

    public override void TryApplyBoost()
    {
        ReadOnlyCollection<Paint> paints = SortedPaints();

        Paint oldPaint = paints[0];
        Paint newPaint = paints[paints.Count - 1];

        if (paints.Count > _bestNewColorIndex)
        {
            newPaint = paints[paints.Count - _bestNewColorIndex];
        }

        foreach (Island island in _levelDataHolder.Islands)
        {
            if (island.IsDone)
            {
                continue;
            }

            if (island.Paint == oldPaint)
            {
                island.SetPaint(newPaint);
            }

            SwapUnitsPaint(island, oldPaint, newPaint);
            island.TryFinish();
        }

        SwapUnitsPaint(_buferIslands.CurrentIsland, oldPaint, newPaint);
        InvokeBoostApplyedEvent();
    }

    private void SwapUnitsPaint(BaseIsland island, Paint oldPaint, Paint newPaint)
    {
        foreach (PlacementPoint point in island.Points)
        {
            if (point.IsFree == false && point.OccupiedUnit.Paint == oldPaint)
            {
                point.OccupiedUnit.SetPaint(newPaint, _paintMaterials);
            }
        }
    }

    private ReadOnlyCollection<Paint> SortedPaints()
    {
        Dictionary<Paint, int> paintsAmouts = new Dictionary<Paint, int>();

        foreach (Island island in _levelDataHolder.Islands)
        {
            if (island.IsDone == false)
            {
                AddPaintAmount(paintsAmouts, island);
            }
        }

        AddPaintAmount(paintsAmouts, _buferIslands.CurrentIsland);

        return paintsAmouts
                .OrderBy(paintAmout => paintAmout.Value)
                .Select(paintAmout => paintAmout.Key)
                .ToList()
                .AsReadOnly();
    }

    private void AddPaintAmount(Dictionary<Paint, int> paintsAmouts, BaseIsland island)
    {
        foreach (PlacementPoint point in island.Points)
        {
            if (point.IsFree == false)
            {
                AddPaintAmount(paintsAmouts, point.OccupiedUnit.Paint);
            }
        }
    }

    private void AddPaintAmount(Dictionary<Paint, int> paintsAmouts, Paint paint)
    {
        if (paintsAmouts.ContainsKey(paint))
        {
            paintsAmouts[paint]++;
        }
        else
        {
            paintsAmouts.Add(paint, 1);
        }
    }
}
