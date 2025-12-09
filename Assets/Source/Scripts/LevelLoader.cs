using System;
using System.Linq;
using Lean.Localization;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private LevelSettings _levelSettings;
    private UnitsPool _unitsPool;
    private PaintMaterials _materials;
    private LevelProgressTracker _levelProgressTracker;
    private UIZoneSwitcher _uiZoneActivator;
    private LevelObjectsHolder _levelDataHolder;
    private UpgradesProvider _upgradesProvider;
    private LeanToken _currentLevelNumberToken;
    private Transform _unitsLookAtPoint;
    private CustomizationSettingsHolder _customizationSettings;
    private GameProgressStorage _gameProgressStorage;
    private UnitMover _unitMover;
    private Transform _cameraTransform;

    private Level _currentLevel;
    private BuferIslandsHolder _buferIslands;
    private GameObject _mainMenuIslands;

    private int _currentLevelId = 1;

    public event Action LevelStartChanging;
    public event Action LevelChanged;

    public LevelSettingsData CurrentLevelData { get; private set; }
    public int ExtraStarMoveCount => _currentLevel.ExtraStarMoveCount;
    public float ExtraScoreTime => _currentLevel.ExtraScoreTime;
    public float AngryBarSpeed => _currentLevel.AngryBarSpeed;

    public void Initialize(LevelSettings levelSettings, UnitsPool unitsPool, PaintMaterials materials,
                            LevelProgressTracker levelProgressTracker, UIZoneSwitcher uiZoneActivator,
                            LevelObjectsHolder levelDataHolder, BuferIslandsHolder buferIslands,
                            UpgradesProvider upgradesProvider, LeanToken currentLevelNumberToken,
                            Transform unitsLookAtPoint, CustomizationSettingsHolder customizationSettings,
                            GameObject mainMenuIslands, GameProgressStorage gameProgressStorage,
                            UnitMover unitMover, Transform cameraTransform)
    {
        _levelSettings = levelSettings;
        _unitsPool = unitsPool;
        _materials = materials;
        _levelProgressTracker = levelProgressTracker;
        _uiZoneActivator = uiZoneActivator;
        _levelDataHolder = levelDataHolder;
        _buferIslands = buferIslands;
        _upgradesProvider = upgradesProvider;
        _currentLevelNumberToken = currentLevelNumberToken;
        _unitsLookAtPoint = unitsLookAtPoint;
        _customizationSettings = customizationSettings;
        _mainMenuIslands = mainMenuIslands;
        _gameProgressStorage = gameProgressStorage;
        _unitMover = unitMover;
        _cameraTransform = cameraTransform;

        CurrentLevelData = _levelSettings.MainMenuSettings;
    }

    public void LoadMainMenu()
    {
        CurrentLevelData = _levelSettings.MainMenuSettings;
        UnloadCurrentLevel();
        _mainMenuIslands.SetActive(true);
        _uiZoneActivator.SwitchToMainMenuUI();
        _levelProgressTracker.StopTracking();
        LevelChanged?.Invoke();
    }

    public void LoadLevel(int levelId)
    {
        LevelStartChanging?.Invoke();

        _currentLevelId = levelId;
        _currentLevelNumberToken.SetValue(levelId);

        _uiZoneActivator.SwitchToInGameUI();

        UnloadCurrentLevel();
        _mainMenuIslands.SetActive(false);
        CurrentLevelData = _levelSettings.Levels.FirstOrDefault(level => level.Id == levelId);

        _currentLevel = Instantiate(CurrentLevelData.LevelPrefab);
        _currentLevel.Initialize(_unitsPool.Get, _materials, _unitsLookAtPoint,
                                 _customizationSettings, _gameProgressStorage,
                                 _unitMover, _cameraTransform);

        int extraIslandSize = (int)_upgradesProvider.UpgradeStageValue(UpgradeType.BuferIslandSize);
        int islandSize = _currentLevel.BuferIslandSize + extraIslandSize;
        _buferIslands.LoadIsland(islandSize);

        _levelDataHolder.SetLevelData(_currentLevel, CurrentLevelData);
        _levelProgressTracker.StartTracking();

        LevelChanged?.Invoke();
    }

    public void ReloadLastLevel()
    {
        LoadLevel(_currentLevelId);
    }

    public void UnloadCurrentLevel()
    {
        if (_currentLevel != null)
        {
            DestroyImmediate(_currentLevel.gameObject);
        }

        _buferIslands.DeactivateCurrentIsland();
        _unitsPool.ReleaseActiveObjects();

        _currentLevel = null;
    }
}
