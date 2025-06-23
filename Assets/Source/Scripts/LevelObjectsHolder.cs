using System.Collections.Generic;
using UnityEngine;

public class LevelObjectsHolder
{
    public Transform IslandsParent { get; private set; }
    public IEnumerable<Island> Islands { get; private set; } = new List<Island>();
    public LevelSettingsData LevelSettings { get; private set; }

    public void SetLevelData(Transform islandsParent, IReadOnlyCollection<Island> islands, LevelSettingsData levelData)
    {
        IslandsParent = islandsParent;
        Islands = new List<Island>(islands).AsReadOnly();
        LevelSettings = levelData;
    }
}
