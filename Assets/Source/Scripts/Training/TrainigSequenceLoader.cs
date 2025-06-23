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

    public TrainigSequenceLoader(LevelLoader levelLoader, LevelObjectsHolder levelObjectsHolder,
                                BuferIslandsHolder buferIslandsHolder, SelectHandler selectHandler,
                                UnitMover unitMover, Camera mainCamera, BoostButtonActivator bosstButtonActivator,
                                LevelProgressTracker levelProgressTracker)
    {
        _levelLoader = levelLoader;
        _levelObjectsHolder = levelObjectsHolder;
        _selectHandler = selectHandler;
        _unitMover = unitMover;
        _buferIslandsHolder = buferIslandsHolder;
        _mainCamera = mainCamera;
        _boostButtonActivator = bosstButtonActivator;
        _levelProgressTracker = levelProgressTracker;

        _levelLoader.LevelChanged += OnLevelChanged;
    }

    private void OnLevelChanged()
    {
        if (_levelLoader.CurrentLevelData.Id <= 0)
        {
            return;
        }

        if (_levelObjectsHolder.IslandsParent.TryGetComponent(out TrainigSequence trainigSequence))
        {
            trainigSequence.Initialize(_levelObjectsHolder, _buferIslandsHolder, _selectHandler, _unitMover, _mainCamera, _boostButtonActivator, _levelProgressTracker);
            trainigSequence.StartTraining();
        }
    }
}
