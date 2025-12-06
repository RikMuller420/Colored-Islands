using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class PaintAmountReduceBoost : Boost
{
    private LevelObjectsHolder _levelDataHolder;
    private BuferIslandsHolder _buferIslands;
    private UnitMover _unitMover;
    private GameProgressStorage _progressStorage;

    private int _bestNewColorIndex = 2;

    public PaintAmountReduceBoost(LevelObjectsHolder levelDataHolder, BuferIslandsHolder buferIslands,
                                    BoostAmountProvider boostAmountProvider, GameProgressStorage progressStorage,
                                    UnitMover unitMover) : base(boostAmountProvider)
    {
        _levelDataHolder = levelDataHolder;
        _buferIslands = buferIslands;
        _unitMover = unitMover;
        _progressStorage = progressStorage;
    }
    public override BoostType Type => BoostType.ReducePaints;

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
                CustomizationPreferences preference = _progressStorage.GetCustomizationPreference(newPaint);
                island.SetPaint(island.Paint, preference.ColorSample);
            }

            SwapUnitsPaint(island, oldPaint, newPaint);
            island.TryFinish();
        }

        SwapUnitsPaint(_buferIslands.CurrentIsland, oldPaint, newPaint);

        foreach (Island island in _levelDataHolder.Islands)
        {
            _unitMover.OptimizeUnitsPosition(island);
        }

        SpendBoost(Type);
    }

    private void SwapUnitsPaint(BaseIsland island, Paint oldPaint, Paint newPaint)
    {
        foreach (IslandPoint point in island.Points)
        {
            if (point.IsFree == false && point.OccupiedUnit.Paint == oldPaint)
            {
                point.OccupiedUnit.SetPaint(newPaint);
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
        foreach (IslandPoint point in island.Points)
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
