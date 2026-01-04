using UnityEngine;

public class AngryTracker
{
    private LevelObjectsHolder _levelDataHolder;
    private LevelLoader _levelLoader;
    private AngryTrackerBalancer _balancer;
    private UpgradesProvider _upgradesProvider;

    private float _angryValue = 0f;

    private float _angryLimit = 1000f;
    private float _angryByIslandFinish = 6f;
    private float _angryByUnitMove = 10f;
    private float _upgradeMultiplier = 1f;
    private float _angrySpeed = 0.3f;

    public float AngryValue => _angryValue / _angryLimit;

    public AngryTracker(LevelObjectsHolder levelDataHolder, LevelLoader levelLoader, LevelProgressTracker progressTracker,
                        UpgradesProvider upgradesProvider)
    {
        _levelDataHolder = levelDataHolder;
        _levelLoader = levelLoader;
        _upgradesProvider = upgradesProvider;
        _balancer = new AngryTrackerBalancer(progressTracker, levelLoader);
        UpdateUpgradeMultiplier(UpgradeType.SlowDownAngryBar);

        _upgradesProvider.Upgraded += UpdateUpgradeMultiplier;
    }

    public void AddAngryTick()
    {
        float instabilityStep = 0f;

        foreach (Island island in _levelDataHolder.Islands)
        {
            instabilityStep += CalculateIslandInstability(island);
        }

        instabilityStep *= _levelLoader.AngryBarSpeed * _balancer.Value * _upgradeMultiplier * _angrySpeed;
        AddAngry(instabilityStep * Time.deltaTime);
    }

    public void AddUnitsMovedTick(UnitsMoveInfo unitsMoveInfo)
    {
        if (unitsMoveInfo.EndIsland is Island endIsland == false)
        {
            return;
        }

        if (unitsMoveInfo.UnitsPaint != endIsland.Paint)
        {
            float angry = _angryByUnitMove * unitsMoveInfo.Units.Count * _angrySpeed;
            AddAngry(angry);
        }
    }

    public void AddIslandFinishedTick(Island island)
    {
        float angry = island.Points.Count * _angryByIslandFinish;
        AddAngry(-angry);
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
        return island.Points.Count;
    }

    private void UpdateUpgradeMultiplier(UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.SlowDownAngryBar)
        {
            _upgradeMultiplier = _upgradesProvider.UpgradeStageValue(UpgradeType.SlowDownAngryBar);
        }
    }
}
