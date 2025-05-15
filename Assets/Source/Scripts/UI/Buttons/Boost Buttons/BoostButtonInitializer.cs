using System.Collections.Generic;
using UnityEngine;

public class BoostButtonInitializer : MonoBehaviour
{
    [SerializeField] private List<BuferIslandBoostButton> _islandBoostButtons = new();
    [SerializeField] private List<ObjectivesFreezeBoostButton> _objectivesFreezeButtons = new();
    [SerializeField] private List<PaintAmountReduceBoostButton> _paintAmountReduceBoostButtons = new();

    public void InitializeButtons(BufferIslandBoost bufferIslandBoost, ObjectivesFreezeBoost objectivesFreezeBoost,
                                  PaintAmountReduceBoost paintAmountReduceBoost)
    {
        foreach (BuferIslandBoostButton islandBoostButton in _islandBoostButtons)
        {
            islandBoostButton.Initialize(bufferIslandBoost);
        }

        foreach (ObjectivesFreezeBoostButton objectivesFreezeButton in _objectivesFreezeButtons)
        {
            objectivesFreezeButton.Initialize(objectivesFreezeBoost);
        }

        foreach (PaintAmountReduceBoostButton paintAmountReduceButtons in _paintAmountReduceBoostButtons)
        {
            paintAmountReduceButtons.Initialize(paintAmountReduceBoost);
        }
    }
}
