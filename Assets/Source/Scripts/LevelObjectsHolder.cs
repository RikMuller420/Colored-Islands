using System.Collections.Generic;

public class LevelObjectsHolder
{
    public IEnumerable<Island> Islands { get; private set; } = new List<Island>();
    public LevelSettingsData LevelSettings { get; private set; }

    public void SetLevelData(IReadOnlyCollection<Island> islands, LevelSettingsData levelData)
    {
        Islands = new List<Island>(islands).AsReadOnly();
        LevelSettings = levelData;
    }
}
