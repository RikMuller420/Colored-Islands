using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private CameraTargets _cameraTargetsVertical;
    [SerializeField] private CameraTargets _cameraTargetsHorizontal;

    [SerializeField] private MeshRenderer _levelBounds;
    [SerializeField] private List<IslandInitializer> _islands = new List<IslandInitializer>();

    public CameraTargets CameraTargetsVertical => _cameraTargetsVertical;
    public CameraTargets CameraTargetsHorizontal => _cameraTargetsHorizontal;

    public MeshRenderer LevelBounds => _levelBounds;
    public IReadOnlyCollection<Island> Islands => _islands.Select(initializer => initializer.Island).ToList().AsReadOnly();

    public void Initialize(Func<Unit> createUnit, PaintMaterials materials, Transform unitsLookAtPoint,
                           CustomizationSettingsHolder customizationSettings)
    {
        foreach (IslandInitializer island in _islands)
        {
            island.Initialize(createUnit, materials, unitsLookAtPoint, customizationSettings);
        }
    }

#if UNITY_EDITOR
    public void SetIslands(List<IslandInitializer> islands)
    {
        _islands = islands;
    }

    public void SetLevelBounds(MeshRenderer levelBounds)
    {
        _levelBounds = levelBounds;
    }

    public void SetCameraTargets(CameraTargets cameraTargetsVertical, CameraTargets cameraTargetsHorizontal)
    {
        _cameraTargetsVertical = cameraTargetsVertical;
        _cameraTargetsHorizontal = cameraTargetsHorizontal;
    }
#endif
}
