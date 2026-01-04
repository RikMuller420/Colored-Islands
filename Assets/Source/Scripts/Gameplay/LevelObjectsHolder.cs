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

    public int BuferIslandSize => _level.BuferIslandSize;
    public int ExtraStarMoveCount => _level.ExtraStarMoveCount;
    public float ExtraScoreTime => _level.ExtraScoreTime;
    public float AngryBarSpeed => _level.AngryBarSpeed;

    public void SetLevelData(Level level, LevelSettingsData levelData)
    {
        _level = level;
        LevelSettings = levelData;
    }
}
