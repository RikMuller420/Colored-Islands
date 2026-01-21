using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Gameplay.Units;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using UnityEngine;

namespace SlimeGround.Gameplay.AngryBar
{
	public class AngryTracker
	{
	    private ILevelData _currentLevelData;
	    private AngryTrackerBalancer _balancer;
	    private IUpgradesData _upgradesData;

	    private float _angryValue = 0f;

	    private float _angryLimit = 1000f;
	    private float _angryByIslandFinish = 6f;
	    private float _angryByUnitMove = 10f;
	    private float _upgradeMultiplier = 1f;
	    private float _angrySpeed = 0.3f;

	    public float AngryValue => _angryValue / _angryLimit;

	    public AngryTracker(ILevelData currentLevelData, LevelProgressTracker progressTracker,
	                        IUpgradesData upgradesData, LevelChangeEventTracker levelChangeEventTracker)
	    {
	        _currentLevelData = currentLevelData;
	        _upgradesData = upgradesData;
	        _balancer = new AngryTrackerBalancer(progressTracker, levelChangeEventTracker);
	        UpdateUpgradeMultiplier(UpgradeType.SlowDownAngryBar);

	        _upgradesData.Upgraded += UpdateUpgradeMultiplier;
	    }

	    public void AddAngryTick()
	    {
	        float instabilityStep = 0f;

	        foreach (Island island in _currentLevelData.Islands)
	        {
	            instabilityStep += CalculateIslandInstability(island);
	        }

	        instabilityStep *= _currentLevelData.AngryBarSpeed * _balancer.Value * _upgradeMultiplier * _angrySpeed;
	        AddAngry(instabilityStep * Time.deltaTime);
	    }

	    public void AddUnitsMovedTick(UnitsMoveInfo unitsMoveInfo)
	    {
	        if (unitsMoveInfo.EndIsland is Island endIsland == false)
	        {
	            return;
	        }

	        if (unitsMoveInfo.UnitsSlot != endIsland.RequredUnitSlot)
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
	            _upgradeMultiplier = _upgradesData.UpgradeStageValue(UpgradeType.SlowDownAngryBar);
	        }
	    }
	}
}
