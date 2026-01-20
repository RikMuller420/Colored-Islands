using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int _buferIslandSize = 3;
    [SerializeField] private int _extraStarMoveCount = 20;
    [SerializeField] private float _extraScoreTime = 60;
    [SerializeField] private float _angryBarSpeed = 1;

    [SerializeField] private CameraTargets _cameraTargetsVertical;
    [SerializeField] private CameraTargets _cameraTargetsHorizontal;

    [SerializeField] private MeshRenderer _levelBounds;
    [SerializeField] private float _unitScale = 2f;
    [SerializeField] private List<IslandInitializer> _islands = new List<IslandInitializer>();
    [SerializeField] private List<Ice> _ices = new List<Ice>();

    public int BuferIslandSize => _buferIslandSize;
    public int ExtraStarMoveCount => _extraStarMoveCount;
    public float ExtraScoreTime => _extraScoreTime;
    public float AngryBarSpeed => _angryBarSpeed;

    public CameraTargets CameraTargetsVertical => _cameraTargetsVertical;
    public CameraTargets CameraTargetsHorizontal => _cameraTargetsHorizontal;

    public MeshRenderer LevelBounds => _levelBounds;
    public IReadOnlyCollection<Island> Islands => _islands.Select(initializer => initializer.Island).ToList().AsReadOnly();

    public void Initialize(Func<Unit> createUnit, PaintMaterials materials, Transform unitsLookAtPoint,
                           CustomizationSettingsHolder customizationSettings, IPlayerData playerData,
                           UnitMover unitMover, Transform cameraTransform)
    {

        foreach (IslandInitializer island in _islands)
        {
            CustomizationPreferences customizationPreferences = playerData.GetCustomizationPreference(island.UnitSlot);
            island.Initialize(createUnit, materials, unitsLookAtPoint, customizationSettings,
                              customizationPreferences.ColorSample, _unitScale);
        }

        foreach (Ice ice in _ices)
        {
            ice.Initialize(unitMover, cameraTransform);
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
