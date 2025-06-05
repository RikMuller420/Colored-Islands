using UnityEngine;

public class AngryTracker
{
    private LevelObjectsHolder _levelDataHolder;

    private float _angryValue = 0f;

    private float _angryLimit = 100000f;
    private float _angryByIslandFinish = 100f;
    private float _angryByUnitMove = 10f;

    public float AngryValue => _angryValue / _angryLimit;

    public AngryTracker(LevelObjectsHolder levelDataHolder)
    {
        _levelDataHolder = levelDataHolder;
    }

    public void AddAngryTick()
    {
        float instabilityStep = 0f;

        foreach (Island island in _levelDataHolder.Islands)
        {
            instabilityStep += CalculateIslandInstability(island);
        }

        AddAngry(instabilityStep * Time.deltaTime);
    }

    public void AddUnitsMovedTick(UnitsMoveInfo unitsMoveInfo)
    {
        if (unitsMoveInfo.EndIsland is Island endIsland == false)
        {
            return;
        }

        float angry = _angryByUnitMove * unitsMoveInfo.Units.Count;
        int angrySign = unitsMoveInfo.UnitsPaint == endIsland.Paint ? -1 : 1;
        AddAngry(angrySign * angry);
    }

    public void AddIslandFinishedTick(Island island)
    {
        AddAngry(-_angryByIslandFinish);
    }

    public void ResetAngryValue()
    {
        _angryValue = 0f;
    }

    private void AddAngry(float value)
    {
        _angryValue += value;
        _angryValue = Mathf.Clamp(_angryValue, 0, _angryLimit);
    }

    private float CalculateIslandInstability(Island island)
    {
        float instability = 0f;

        foreach (IslandPoint point in island.Points)
        {
            if (point.IsFree == false && point.OccupiedUnit.Paint != island.Paint)
            {
                instability += 1f;
            }
        }

        return instability;
    }
}
