using System.Collections.Generic;
using UnityEngine;

public class BoostButtonInitializer : MonoBehaviour
{
    [SerializeField] private List<BuferIslandBoostButton> _islandBoostButtons = new();
    [SerializeField] private List<ObjectivesFreezeBoostButton> _objectivesFreezeButtons = new();

    public void InitializeButtons(BufferIslandBoost bufferIslandBoost, ObjectivesFreezeBoost objectivesFreezeBoost)
    {
        foreach (BuferIslandBoostButton islandBoostButton in _islandBoostButtons)
        {
            islandBoostButton.Initialize(bufferIslandBoost);
        }

        foreach (ObjectivesFreezeBoostButton objectivesFreezeButton in _objectivesFreezeButtons)
        {
            objectivesFreezeButton.Initialize(objectivesFreezeBoost);
        }
    }
}
