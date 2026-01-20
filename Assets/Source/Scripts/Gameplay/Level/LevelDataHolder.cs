using System.Collections.Generic;
using UnityEngine;

public class LevelDataHolder: ILevelData
{
    public Level Level { get; private set; }

    public Transform IslandsParent => Level?.transform;
    public IEnumerable<Island> Islands => Level?.Islands;
    public CameraTargets VerticalCameraTargets => Level?.CameraTargetsVertical;
    public CameraTargets HorizontalCameraTargets => Level?.CameraTargetsHorizontal;
    public MeshRenderer LevelBounds => Level?.LevelBounds;

    public int LevelId => _levelSettings.Id;
    public int ExtraStarMoveCount => Level.ExtraStarMoveCount;
    public float ExtraScoreTime => Level.ExtraScoreTime;
    public int BuferIslandSize => Level.BuferIslandSize;
    public float AngryBarSpeed => Level.AngryBarSpeed;

    private LevelSettingsData _levelSettings;

    public LevelDataHolder(LevelSettingsData levelData)
    {
        SetLevelData(null, levelData);
    }

    public void SetLevelData(Level level, LevelSettingsData levelData)
    {
        Level = level;
        _levelSettings = levelData;
    }
}
