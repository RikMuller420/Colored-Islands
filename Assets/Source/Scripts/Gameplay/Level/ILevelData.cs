using System.Collections.Generic;
using UnityEngine;

public interface ILevelData
{
    public Transform IslandsParent { get; }
    public IEnumerable<Island> Islands { get; }
    public CameraTargets VerticalCameraTargets { get; }
    public CameraTargets HorizontalCameraTargets { get; }
    public MeshRenderer LevelBounds { get; }

    public int LevelId { get; }
    public int ExtraStarMoveCount { get; }
    public float ExtraScoreTime { get; }
    public int BuferIslandSize { get; }
    public float AngryBarSpeed { get; }
}
