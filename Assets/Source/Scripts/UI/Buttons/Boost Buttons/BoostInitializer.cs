using System.Collections.Generic;
using UnityEngine;

public class BoostInitializer : MonoBehaviour
{
    [SerializeField] private LayerMask _paintedIslands;
    [SerializeField] private BuferIslandsHolder _buferIslands;
    [SerializeField] private PaintMaterials _materials;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private LevelProgressTracker _levelProgressTracker;

    [SerializeField] private List<BoostButton> _buferIslandBoostButtons = new();
    [SerializeField] private List<BoostButton> _objectivesFreezeButtons = new();
    [SerializeField] private List<BoostButton> _paintAmountReduceBoostButtons = new();
    [SerializeField] private List<BoostButton> _islandFinishBoostButtons = new();

    public void Initialize(UnitMover unitMover, ClickHandler gameClickHandler, SelectHandler selectHandler,
                           LevelObjectsHolder levelDataHolder)
    {

        var islandInstantFinisher = new IslandFinishBehaviour(levelDataHolder, _buferIslands, unitMover, _paintedIslands);

        var islandFinishBoost = new IslandFinishBoost(selectHandler, gameClickHandler, islandInstantFinisher,
                                                      _levelLoader);
        var bufferIslandBoost = new BufferIslandBoost(_buferIslands, unitMover);
        var objectivesFreezeBoost = new ObjectivesFreezeBoost(_levelProgressTracker, unitMover, _levelLoader);
        var paintAmountReduceBoost = new PaintAmountReduceBoost(levelDataHolder, _buferIslands, _materials);

        Dictionary<Boost, IEnumerable<BoostButton>> boostsButtons = new Dictionary<Boost, IEnumerable<BoostButton>>()
        {
            { bufferIslandBoost, _buferIslandBoostButtons },
            { objectivesFreezeBoost, _objectivesFreezeButtons },
            { paintAmountReduceBoost, _paintAmountReduceBoostButtons },
            { islandFinishBoost, _islandFinishBoostButtons }
        };

        var boostAvailabilityUpdater = new BoostAvailabilityUpdater(boostsButtons, _levelLoader);
        InitializeButtons(boostsButtons);
    }

    private void InitializeButtons(Dictionary<Boost, IEnumerable<BoostButton>> boostsButtons)
    {
        foreach (var boostButton in boostsButtons)
        {
            foreach (BoostButton button in boostButton.Value)
            {
                button.Initialize(boostButton.Key);
            }
        }
    }
}
