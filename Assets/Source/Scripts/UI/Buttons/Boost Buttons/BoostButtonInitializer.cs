using System.Collections.Generic;
using UnityEngine;

public class BoostButtonInitializer : MonoBehaviour
{
    [SerializeField] private List<BuferIslandBoostButton> _islandBoostButtons = new();
    [SerializeField] private List<ObjectivesFreezeBoostButton> _objectivesFreezeButtons = new();
    [SerializeField] private List<PaintAmountReduceBoostButton> _paintAmountReduceBoostButtons = new();
    [SerializeField] private List<IslandFinishBoostButton> _islandFinishBoostButtons = new();

    public void InitializeButtons(BufferIslandBoost bufferIslandBoost, ObjectivesFreezeBoost objectivesFreezeBoost,
                                  PaintAmountReduceBoost paintAmountReduceBoost, IslandFinishBoost islandFinishBoost)
    {
        foreach (BuferIslandBoostButton islandBoostButton in _islandBoostButtons)
        {
            islandBoostButton.Initialize(bufferIslandBoost);
        }

        foreach (ObjectivesFreezeBoostButton objectivesFreezeButton in _objectivesFreezeButtons)
        {
            objectivesFreezeButton.Initialize(objectivesFreezeBoost);
        }

        foreach (PaintAmountReduceBoostButton paintAmountReduceButton in _paintAmountReduceBoostButtons)
        {
            paintAmountReduceButton.Initialize(paintAmountReduceBoost);
        }

        foreach (IslandFinishBoostButton islandFinishButton in _islandFinishBoostButtons)
        {
            islandFinishButton.Initialize(islandFinishBoost);
        }
    }
}
