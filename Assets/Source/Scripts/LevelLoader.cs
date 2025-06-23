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
    private GameProgressStorage _gameProgressStorage;
    private LeanToken _currentLevelNumberToken;
    private Transform _unitsLookAtPoint;
    private CustomizationSettingsHolder _customizationSettings;

    private IslandsGroupInitializer _currentIslands;
    private BuferIslandsHolder _buferIslands;

    private int _currentLevelId = 1;

    public event Action LevelChanged;

    public LevelSettingsData CurrentLevelData { get; private set; }

    public void Initialize(LevelSettings levelSettings, UnitsPool unitsPool, PaintMaterials materials,
                            LevelProgressTracker levelProgressTracker, UIZoneSwitcher uiZoneActivator,
                            LevelObjectsHolder levelDataHolder, BuferIslandsHolder buferIslands,
                            GameProgressStorage gameProgressStorage, LeanToken currentLevelNumberToken,
                            Transform unitsLookAtPoint, CustomizationSettingsHolder customizationSettings)
    {
        _levelSettings = levelSettings;
        _unitsPool = unitsPool;
        _materials = materials;
        _levelProgressTracker = levelProgressTracker;
        _uiZoneActivator = uiZoneActivator;
        _levelDataHolder = levelDataHolder;
        _buferIslands = buferIslands;
        _gameProgressStorage = gameProgressStorage;
        _currentLevelNumberToken = currentLevelNumberToken;
        _unitsLookAtPoint = unitsLookAtPoint;
        _customizationSettings = customizationSettings;

        CurrentLevelData = _levelSettings.MainMenuSettings;
    }

    public void LoadMainMenu()
    {
        CurrentLevelData = _levelSettings.MainMenuSettings;
        UnloadCurrentLevel();
        _uiZoneActivator.SwitchToMainMenuUI();
        _levelProgressTracker.StopTracking();
        LevelChanged?.Invoke();
    }

    public void LoadLevel(int levelId)
    {
        _currentLevelId = levelId;
        _currentLevelNumberToken.SetValue(levelId);

        _uiZoneActivator.SwitchToInGameUI();

        UnloadCurrentLevel();
        CurrentLevelData = _levelSettings.Levels.FirstOrDefault(level => level.Id == levelId);

        _currentIslands = Instantiate(CurrentLevelData.LevelPrefab);
        _currentIslands.Initialize(_unitsPool.Get, _materials, _unitsLookAtPoint, _customizationSettings);

        int extraIslandSize = _gameProgressStorage.GetUpgradeStage(UpgradeType.BuferIslandSize);
        int islandSize = CurrentLevelData.BuferIslandSize + extraIslandSize;
        _buferIslands.LoadIsland(islandSize);

        _levelDataHolder.SetLevelData(_currentIslands.transform, _currentIslands.Islands, CurrentLevelData);
        _levelProgressTracker.StartTracking();

        LevelChanged?.Invoke();
    }

    public void ReloadLastLevel()
    {
        LoadLevel(_currentLevelId);
    }

    public void UnloadCurrentLevel()
    {
        if (_currentIslands != null)
        {
            Destroy(_currentIslands.gameObject);
        }

        _buferIslands.DeactivateCurrentIsland();
        _unitsPool.ReleaseActiveObjects();

        _currentIslands = null;
    }
}
