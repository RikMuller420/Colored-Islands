using SlimeGround.Core;
using SlimeGround.Core.InputHandling;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Gameplay.Units;
using SlimeGround.Menu.Extensions.Windows;
using SlimeGround.Menu.OrientationChanger;
using SlimeGround.Menu.Windows.Customization;
using SlimeGround.Menu.Windows.FinalScore;
using UnityEngine;

namespace SlimeGround.Gameplay.Training
{
	public class TrainigSequenceLoader : MonoBehaviour
	{
	    [SerializeField] private LevelSettings _levelSettings;

	    [SerializeField] private LevelLoader _levelLoader;
	    [SerializeField] private BuferIslandsHolder _buferIslandsHolder;
	    [SerializeField] private Camera _mainCamera;
	    [SerializeField] private BoostButtonActivator _boostButtonActivator;
	    [SerializeField] private LevelProgressTracker _levelProgressTracker;
	    [SerializeField] private UIOrientationChanger _uIOrientationChanger;
	    [SerializeField] private PlayerDataProvider _playerData;
	    [SerializeField] private MenuWindow _inGameMenu;
	    [SerializeField] private FinalScoreWindow _finalScoreWindow;
	    [SerializeField] private MenuTrainigSequence _menuTrainigSequence;
	    [SerializeField] private ScreenSizeChangeTracker _screenSizeChangeTracker;
	    [SerializeField] private Canvas _canvas;
	    [SerializeField] private CustomizationWindow _customizationMenu;

	    private IUnitMovedEvent _unitMovedEvent;
	    private IUnitsSelectedEvent _unitsSelectedEvent;

	    public void Initilize(IUnitsSelectedEvent unitsSelectedEvent, IUnitMovedEvent unitMovedEvent)
	    {
	        _unitsSelectedEvent = unitsSelectedEvent;
	        _unitMovedEvent = unitMovedEvent;

	        _levelLoader.LevelChanged += OnLevelChanged;
	    }

		public void Dispose()
		{
			_levelLoader.LevelChanged -= OnLevelChanged;
		}

	    public void TryLoadTrainingLevel()
	    {
	        if (_playerData.LastAvailableLevelId <= _levelSettings.LastTrainingLevel)
	        {
	            _levelLoader.LoadLevel(_playerData.LastAvailableLevelId);
	        }
	    }

	    private void OnLevelChanged(ILevelData levelData)
	    {
	        if (levelData.LevelId == _levelSettings.MainMenuSettings.Id)
	        {
	            return;
	        }

	        if (levelData.IslandsParent.TryGetComponent(out TrainigSequence trainigSequence))
	        {
	            trainigSequence.Initialize(levelData, _buferIslandsHolder, _unitsSelectedEvent, _unitMovedEvent, _mainCamera,
	                                       _boostButtonActivator, _levelProgressTracker, _uIOrientationChanger, _playerData,
	                                       _inGameMenu, _finalScoreWindow, _menuTrainigSequence, _screenSizeChangeTracker,
	                                       _canvas, _levelLoader, _customizationMenu);
	            trainigSequence.StartTrainingNextFrame();
	        }
	    }
	}
}
