using System.Collections.Generic;
using UnityEngine;

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
