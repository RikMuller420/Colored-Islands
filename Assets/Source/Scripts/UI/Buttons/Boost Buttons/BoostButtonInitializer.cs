using System.Collections.Generic;
using UnityEngine;

public class BoostButtonInitializer : MonoBehaviour
{
    [SerializeField] private List<BuferIslandBoostButton> _islandBoostButtons = new();

    public void InitializeButtons(BufferIslandBoost bufferIslandBoost)
    {
        foreach (BuferIslandBoostButton islandBoostButton in _islandBoostButtons)
        {
            islandBoostButton.Initialize(bufferIslandBoost);
        }
    }
}
