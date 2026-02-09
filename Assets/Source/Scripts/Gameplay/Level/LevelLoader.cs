using System;
using System.Linq;
using Lean.Localization;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Data.ScriptableObjects.Paints;
using SlimeGround.Data.ScriptableObjects.UnitFaces;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Units;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Windows;
using SlimeGround.Menu.Windows.Customization;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using UnityEngine;

namespace SlimeGround.Gameplay.Levels
{
	public class LevelLoader : MonoBehaviour
	{
	    [SerializeField] private UnitsFaceSettings _faceSettings;
	    [SerializeField] private UnitsHatSettings _hatSettings;
	    [SerializeField] private LevelSettings _levelSettings;
	    [SerializeField] private ColorSampleMaterials _materials;

	    [SerializeField] private UnitsPool _unitsPool;
	    [SerializeField] private UIZoneSwitcher _uiZoneActivator;
	    [SerializeField] private BuferIslandsHolder _buferIslands;
	    [SerializeField] private LeanToken _currentLevelNumberToken;
	    [SerializeField] private Transform _unitsLookAtPoint;
	    [SerializeField] private GameObject _mainMenuIslands;
	    [SerializeField] private Transform _cameraTransform;
	    [SerializeField] private PlayerDataProvider _playerData;

	    private IUpgradesData _upgradesData;
	    private CustomizationSettingsHolder _customizationSettings;
	    private UnitMover _unitMover;
	    private LevelDataHolder _currentLevelData;

	    public event Action LevelStartChanging;
	    public event Action<ILevelData> LevelChanged;

	    public void Initialize(IUpgradesData upgradesData, UnitMover unitMover,
	                           LevelDataHolder levelDataHolder)
	    {
	        _customizationSettings = new CustomizationSettingsHolder(_materials, _playerData, _faceSettings, _hatSettings);
	        _currentLevelData = levelDataHolder;
	        _upgradesData = upgradesData;
	        _unitMover = unitMover;
	    }

		public void Dispose()
		{
			_customizationSettings.Dispose();
		}

	    public void LoadMainMenu()
	    {
	        UnloadCurrentLevel();
	        _mainMenuIslands.SetActive(true);
	        _uiZoneActivator.SwitchToMainMenuUI();
	        _currentLevelData.SetLevelData(null, _levelSettings.MainMenuSettings);

	        LevelChanged?.Invoke(_currentLevelData);
	    }

	    public void LoadLevel(int levelId)
	    {
	        LevelStartChanging?.Invoke();

	        LevelSettingsData levelData = _levelSettings.Levels.FirstOrDefault(level => level.Id == levelId);

	        _currentLevelNumberToken.SetValue(levelId);
	        _uiZoneActivator.SwitchToInGameUI();

	        UnloadCurrentLevel();
	        _mainMenuIslands.SetActive(false);

	        Level level = Instantiate(levelData.LevelPrefab);
	        level.Initialize(_unitsPool.Get, _materials, _unitsLookAtPoint,
	                         _customizationSettings, _playerData,
	                         _unitMover, _cameraTransform);

	        _currentLevelData.SetLevelData(level, levelData);
	        int extraIslandSize = (int)_upgradesData.UpgradeStageValue(UpgradeType.BuferIslandSize);
	        int islandSize = _currentLevelData.BuferIslandSize + extraIslandSize;
	        _buferIslands.LoadIsland(islandSize);

	        LevelChanged?.Invoke(_currentLevelData);
	        MetricSaver.StartLevel();
	    }

	    public void ReloadLastLevel()
	    {
	        LoadLevel(_currentLevelData.LevelId);
	    }

	    public void UnloadCurrentLevel()
	    {
	        if (_currentLevelData.Level != null)
	        {
	            DestroyImmediate(_currentLevelData.Level.gameObject);
	        }

	        _buferIslands.DeactivateCurrentIsland();
	        _unitsPool.ReleaseActiveUnits();
	    }
	}
}
