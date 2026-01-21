using System.Collections.Generic;
using SlimeGround.Core.InputHandling;
using SlimeGround.Data.Saves;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Gameplay.Units;
using SlimeGround.Integration.Ads;
using SlimeGround.Menu;
using SlimeGround.Menu.Boosts;
using UnityEngine;

namespace SlimeGround.Gameplay.Boosts
{
	public class BoostInitializer : MonoBehaviour
	{
	    [SerializeField] private PlayerDataProvider _playerData;
	    [SerializeField] private LayerMask _paintedIslands;
	    [SerializeField] private BuferIslandsHolder _buferIslands;
	    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
	    [SerializeField] private LevelProgressTracker _levelProgressTracker;

	    [SerializeField] private BoostViewInitializer _boostViewInitializer;

	    public ObjectivesFreezeBoost ObjectivesFreezeBoost { get; private set; }

	    public void Initialize(UnitMover unitMover, ClickHandler clickHandler,
	                           ILevelData currentLevelData, BoostAmountProvider boostAmountProvider,
	                           WalletProvider walletProvider,
	                           RewardedAdProvider rewardedAdProvider, out IBoostStopApplyedEvent freezeBoostApplyed)
	    {
	        var islandInstantFinisher = new IslandFinishClickBehaviour(currentLevelData, _buferIslands, unitMover, _paintedIslands);

	        var islandFinishBoost = new IslandFinishBoost(clickHandler, islandInstantFinisher,
	                                                      _levelChangeEventTracker, boostAmountProvider);
	        var bufferIslandBoost = new BufferIslandBoost(_buferIslands, boostAmountProvider);

	        var objectivesFreezeBoost = new ObjectivesFreezeBoost(_levelProgressTracker, unitMover, _levelChangeEventTracker, boostAmountProvider);
	        freezeBoostApplyed = objectivesFreezeBoost;

	        var paintAmountReduceBoost = new PaintAmountReduceBoost(currentLevelData, _buferIslands, boostAmountProvider,
	                                                                _playerData, unitMover);

	        IEnumerable<Boost> boosts = new List<Boost>()
	        {
	            bufferIslandBoost,
	            objectivesFreezeBoost,
	            paintAmountReduceBoost,
	            islandFinishBoost
	        };

	        _boostViewInitializer.Initialize(boosts, boostAmountProvider, walletProvider, islandInstantFinisher, rewardedAdProvider);
	    }
	}
}
