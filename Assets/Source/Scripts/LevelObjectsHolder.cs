using System.Collections.Generic;
using UnityEngine;

public class LevelObjectsHolder
{
    private Level _level;

    public Transform IslandsParent => _level?.transform;
    public IEnumerable<Island> Islands => _level?.Islands;
    public CameraTargets VerticalCameraTargets => _level?.CameraTargetsVertical;
    public CameraTargets HorizontalCameraTargets => _level?.CameraTargetsHorizontal;
    public MeshRenderer LevelBounds => _level?.LevelBounds;
    public LevelSettingsData LevelSettings { get; private set; }

    public void SetLevelData(Level level, LevelSettingsData levelData)
    {
        _level = level;
        LevelSettings = levelData;
    }
}
