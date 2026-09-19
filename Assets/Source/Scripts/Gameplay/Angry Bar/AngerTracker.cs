using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Gameplay.Units;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using UnityEngine;

namespace SlimeGround.Gameplay.AngryBar
{
	public class AngerTracker
	{
	    private const float AngerLimit = 1000f;
	    private const float AngerByIslandFinish = 6f;
	    private const float AngerByUnitMove = 10f;
	    private const float AngerSpeed = 0.3f;

		private readonly ILevelData _currentLevelData;
		private readonly AngerTrackerBalancer _balancer;
		private readonly IUpgradesData _upgradesData;

		private float _upgradeMultiplier = 1f;
		private float _angerValue = 0f;

		public AngerTracker(ILevelData currentLevelData, LevelProgressTracker progressTracker,
	                        IUpgradesData upgradesData, LevelChangeEventTracker levelChangeEventTracker)
	    {
	        _currentLevelData = currentLevelData;
	        _upgradesData = upgradesData;
	        _balancer = new AngerTrackerBalancer(progressTracker, levelChangeEventTracker);
	        UpdateUpgradeMultiplier(UpgradeType.SlowDownAngryBar);

	        _upgradesData.Upgraded += UpdateUpgradeMultiplier;
	    }

		public float AngryValue => _angerValue / AngerLimit;

		public void Dispose()
		{
			_balancer.Dispose();
		}

		public void AddAngryTick()
	    {
	        float instabilityStep = 0f;

	        foreach (Island island in _currentLevelData.Islands)
	        {
	            instabilityStep += CalculateIslandInstability(island);
	        }

	        instabilityStep *= _currentLevelData.AngryBarSpeed * _balancer.Value * _upgradeMultiplier * AngerSpeed;
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
	            float angry = AngerByUnitMove * unitsMoveInfo.Units.Count * AngerSpeed;
	            AddAngry(angry);
	        }
	    }

	    public void AddIslandFinishedTick(Island island)
	    {
	        float angry = island.Points.Count * AngerByIslandFinish;
	        AddAngry(-angry);
	    }

	    public void ResetAngryValue()
	    {
	        _angerValue = 0f;
	    }

	    private void AddAngry(float value)
	    {
	        _angerValue += value;
	        _angerValue = Mathf.Clamp(_angerValue, 0, AngerLimit);
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
