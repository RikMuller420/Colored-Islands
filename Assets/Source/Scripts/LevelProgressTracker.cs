using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressTracker
{
    private IReadOnlyCollection<Island> _islands = new List<Island>();

    public event Action LevelFinished;

    public void StartTrack(IslandsGroupInitializer islandsGroup, LevelSettings levelSettings)
    {
        StopTrack();

        _islands = islandsGroup.Islands;

        foreach (Island island in _islands)
        {
            island.IslandFinished += CheckFinishLevel;
        }
    }

    public void StopTrack()
    {
        foreach (Island island in _islands)
        {
            island.IslandFinished -= CheckFinishLevel;
        }
    }

    private void CheckFinishLevel()
    {
        foreach (Island island in _islands)
        {
            if (island.IsDone == false)
            {
                return;
            }
        }

        StopTrack();
        LevelFinished?.Invoke();
    }
}
