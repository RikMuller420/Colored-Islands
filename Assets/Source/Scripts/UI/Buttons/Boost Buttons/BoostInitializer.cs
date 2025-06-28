using System.Collections.Generic;
using UnityEngine;

public class BoostInitializer : MonoBehaviour
{
    [SerializeField] private LayerMask _paintedIslands;
    [SerializeField] private BuferIslandsHolder _buferIslands;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private LevelProgressTracker _levelProgressTracker;
    [SerializeField] private BoostBuyConfirmationWindow boostBuyWindow;
    [SerializeField] private GameplaySoundPlayer _gameplaySoundPlayer;
    [SerializeField] private MenuWindow _outOfBoostWindow;

    [SerializeField] private BoostButton _buferIslandBoostButton;
    [SerializeField] private BoostButton _objectivesFreezeButton;
    [SerializeField] private BoostButton _paintAmountReduceBoostButton;
    [SerializeField] private BoostButton _islandFinishBoostButton;

    public ObjectivesFreezeBoost ObjectivesFreezeBoost { get; private set; }

    public void Initialize(UnitMover unitMover, ClickHandler gameClickHandler, SelectHandler selectHandler,
                           LevelObjectsHolder levelDataHolder, BoostAmountProvider boostAmountProvider)
    {
        var islandInstantFinisher = new IslandFinishBehaviour(levelDataHolder, _buferIslands, unitMover, _paintedIslands);

        var islandFinishBoost = new IslandFinishBoost(selectHandler, gameClickHandler, islandInstantFinisher,
                                                      _levelLoader, boostAmountProvider);
        var bufferIslandBoost = new BufferIslandBoost(_buferIslands, unitMover, boostAmountProvider);
        ObjectivesFreezeBoost = new ObjectivesFreezeBoost(_levelProgressTracker, unitMover, _levelLoader, boostAmountProvider);
        var paintAmountReduceBoost = new PaintAmountReduceBoost(levelDataHolder, _buferIslands, boostAmountProvider,
                                                                unitMover);

        IEnumerable<Boost> boosts = new List<Boost>()
        {
            bufferIslandBoost,
            ObjectivesFreezeBoost,
            paintAmountReduceBoost,
            islandFinishBoost
        };

        var boostSoundPlayer = new BoostSoundPlayer(_gameplaySoundPlayer, _outOfBoostWindow, boosts);

        Dictionary<Boost, BoostButton> boostsButtons = new Dictionary<Boost, BoostButton>()
        {
            { bufferIslandBoost, _buferIslandBoostButton },
            { ObjectivesFreezeBoost, _objectivesFreezeButton },
            { paintAmountReduceBoost, _paintAmountReduceBoostButton },
            { islandFinishBoost, _islandFinishBoostButton }
        };

        var boostAvailabilityUpdater = new BoostAvailabilityUpdater(boostsButtons, _levelLoader);
        var boostAnimator = new BoostAnimator(boostsButtons);

        foreach (var boostButton in boostsButtons)
        {
            boostButton.Value.Initialize(boostButton.Key, boostAmountProvider, boostBuyWindow);
        }
    }
}
