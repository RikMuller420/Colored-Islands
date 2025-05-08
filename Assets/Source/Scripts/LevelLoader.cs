using System;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private LevelSettings _levelSettings;
    private UnitsPool _unitsPool;
    private PaintMaterials _materials;
    private BuferIslandsHolder _buferIslands;
    private LevelProgressTracker _levelProgressTracker;
    private UIZoneSwitcher _uiZoneActivator;

    private IslandsGroupInitializer _currentIslands;
    private BuferIslandInitializer _currentBufferIsland;

    private int _currentLevelId = 1;

    public event Action LevelChanged;

    public LevelSettingsData CurrentLevelData { get; private set; }

    public void Initialize(LevelSettings levelSettings, UnitsPool unitsPool, PaintMaterials materials,
                            BuferIslandsHolder buferIslands, LevelProgressTracker levelProgressTracker,
                            UIZoneSwitcher uiZoneActivator)
    {
        _levelSettings = levelSettings;
        _unitsPool = unitsPool;
        _materials = materials;
        _buferIslands = buferIslands;
        _levelProgressTracker = levelProgressTracker;
        _uiZoneActivator = uiZoneActivator;
        CurrentLevelData = _levelSettings.MainMenuSettings;
    }

    public void LoadMainMenu()
    {
        CurrentLevelData = _levelSettings.MainMenuSettings;
        UnloadCurrentLevel();
        _uiZoneActivator.SwitchToMainMenuUI();
        _levelProgressTracker.StopTrack();
        LevelChanged?.Invoke();
    }

    public void LoadLevel(int levelId)
    {
        _currentLevelId = levelId;
        _uiZoneActivator.SwitchToInGameUI();

        UnloadCurrentLevel();
        CurrentLevelData = _levelSettings.Levels.FirstOrDefault(level => level.Id == levelId);

        _currentIslands = Instantiate(CurrentLevelData.LevelPrefab);
        _currentIslands.Initialize(_unitsPool.Get, _materials);

        _currentBufferIsland = _buferIslands.GetIsland(CurrentLevelData.BuferIslandSize);
        _currentBufferIsland.Initialize();

        _levelProgressTracker.StartTrack(_currentIslands, CurrentLevelData);

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

        if (_currentBufferIsland != null)
        {
            _currentBufferIsland.gameObject.SetActive(false);
        }

        _unitsPool.ReleaseActiveObjects();

        _currentIslands = null;
        _currentBufferIsland = null;
    }
}
