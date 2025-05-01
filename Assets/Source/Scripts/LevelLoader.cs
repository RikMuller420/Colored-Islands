using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private UnitCreator _unitCreator;
    [SerializeField] private PaintMaterials _materials;
    [SerializeField] private BuferIslandsHolder _buferIslands;

    private IslandsGroupInitializer _currentIslands;
    private BuferIslandInitializer _currentBufferIsland;

    private void Start()
    {
        LoadLevel(1);
    }

    public void LoadLevel(int levelId)
    {
        UnloadCurrentLevel();
        LevelSettingsData levelData = _levelSettings.Levels.FirstOrDefault(level => level.Id == levelId);

        _currentIslands = Instantiate(levelData.LevelPrefab);
        _currentIslands.Initialize(_unitCreator.Create, _materials);

        _currentBufferIsland = _buferIslands.GetIsland(levelData.BuferIslandSize);
        _currentBufferIsland.Initialize();
    }

    private void UnloadCurrentLevel()
    {
        if (_currentIslands != null)
        {
            Destroy(_currentIslands.gameObject);
        }

        if (_currentBufferIsland != null)
        {
            _currentBufferIsland.gameObject.SetActive(false);
        }

        _currentIslands = null;
        _currentBufferIsland = null;
    }
}
