using UnityEngine;

public class TrainigSequenceLoader
{
    private LevelLoader _levelLoader;
    private LevelObjectsHolder _levelObjectsHolder;
    private BuferIslandsHolder _buferIslandsHolder;
    private SelectHandler _selectHandler;
    private UnitMover _unitMover;
    private Camera _mainCamera;
    private BoostButtonActivator _boostButtonActivator;
    private LevelProgressTracker _levelProgressTracker;
    private UIOrientationChanger _uIOrientationChanger;
    private GameProgressStorage _progressStorage;
    private MenuWindow _inGameMenu;
    private FinalScoreWindow _finalScoreWindow;
    private MenuTrainigSequence _menuTrainigSequence;
    private LevelSettings _levelSettings;

    public TrainigSequenceLoader(LevelLoader levelLoader, LevelObjectsHolder levelObjectsHolder,
                                BuferIslandsHolder buferIslandsHolder, SelectHandler selectHandler,
                                UnitMover unitMover, Camera mainCamera, BoostButtonActivator bosstButtonActivator,
                                LevelProgressTracker levelProgressTracker, UIOrientationChanger uIOrientationChanger,
                                GameProgressStorage progressStorage, MenuWindow inGameMenu, FinalScoreWindow finalScoreWindow,
                                MenuTrainigSequence menuTrainigSequence, LevelSettings levelSettings)
    {
        _levelLoader = levelLoader;
        _levelObjectsHolder = levelObjectsHolder;
        _selectHandler = selectHandler;
        _unitMover = unitMover;
        _buferIslandsHolder = buferIslandsHolder;
        _mainCamera = mainCamera;
        _boostButtonActivator = bosstButtonActivator;
        _levelProgressTracker = levelProgressTracker;
        _uIOrientationChanger = uIOrientationChanger;
        _progressStorage = progressStorage;
        _inGameMenu = inGameMenu;
        _finalScoreWindow = finalScoreWindow;
        _menuTrainigSequence = menuTrainigSequence;
        _levelSettings = levelSettings;

        _levelLoader.LevelChanged += OnLevelChanged;
    }

    public void TryLoadTrainingLevel()
    {
        if (_progressStorage.LastAvailableLevelId <= _levelSettings.LastTrainingLevel)
        {
            _levelLoader.LoadLevel(_progressStorage.LastAvailableLevelId);
        }
    }

    private void OnLevelChanged()
    {
        if (_levelLoader.CurrentLevelData.Id <= 0)
        {
            return;
        }

        if (_levelObjectsHolder.IslandsParent.TryGetComponent(out TrainigSequence trainigSequence))
        {
            trainigSequence.Initialize(_levelObjectsHolder, _buferIslandsHolder, _selectHandler, _unitMover, _mainCamera,
                                       _boostButtonActivator, _levelProgressTracker, _uIOrientationChanger, _progressStorage,
                                       _inGameMenu, _finalScoreWindow, _menuTrainigSequence);
            trainigSequence.StartTrainingNextFrame();
        }
    }
}
