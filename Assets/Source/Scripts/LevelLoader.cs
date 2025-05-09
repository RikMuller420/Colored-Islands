using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private LevelSettings _levelSettings;
    private UnitsPool _unitsPool;
    private PaintMaterials _materials;
    private BuferIslandsHolder _buferIslands;
    private LevelProgressTracker _levelProgressTracker;

    private IslandsGroupInitializer _currentIslands;
    private BuferIslandInitializer _currentBufferIsland;

    public void Initialize(LevelSettings levelSettings, UnitsPool unitsPool, PaintMaterials materials,
                            BuferIslandsHolder buferIslands, LevelProgressTracker levelProgressTracker)
    {
        _levelSettings = levelSettings;
        _unitsPool = unitsPool;
        _materials = materials;
        _buferIslands = buferIslands;
        _levelProgressTracker = levelProgressTracker;
    }

    public void LoadLevel(int levelId)
    {
        UnloadCurrentLevel();
        LevelSettingsData levelData = _levelSettings.Levels.FirstOrDefault(level => level.Id == levelId);

        _currentIslands = Instantiate(levelData.LevelPrefab);
        _currentIslands.Initialize(_unitsPool.Get, _materials);

        _currentBufferIsland = _buferIslands.GetIsland(levelData.BuferIslandSize);
        _currentBufferIsland.Initialize();

        _levelProgressTracker.StartTrack(_currentIslands, _levelSettings);
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
        _levelProgressTracker.StopTrack();

        _currentIslands = null;
        _currentBufferIsland = null;
    }
}
