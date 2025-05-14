using System.Collections.Generic;

public class LevelDataHolder
{
    public LevelSettingsData LevelSettings { get; private set; }
    public IReadOnlyCollection<Island> Islands { get; private set; }

    public void SetLevelData(IReadOnlyCollection<Island> islands, LevelSettingsData levelData)
    {
        Islands = new List<Island>(islands).AsReadOnly();
        LevelSettings = levelData;
    }
}
