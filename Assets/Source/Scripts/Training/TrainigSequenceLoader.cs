using UnityEngine.EventSystems;

public class TrainigSequenceLoader
{
    private LevelLoader _levelLoader;
    private LevelObjectsHolder _levelObjectsHolder;
    private BuferIslandsHolder _buferIslandsHolder;
    private SelectHandler _selectHandler;

    public TrainigSequenceLoader(LevelLoader levelLoader, LevelObjectsHolder levelObjectsHolder,
                                BuferIslandsHolder buferIslandsHolder, SelectHandler selectHandler)
    {
        _levelLoader = levelLoader;
        _levelObjectsHolder = levelObjectsHolder;
        _selectHandler = selectHandler;
        _buferIslandsHolder = buferIslandsHolder;

        _levelLoader.LevelChanged += OnLevelChanged;
    }

    private void OnLevelChanged()
    {
        if (_levelLoader.CurrentLevelData.Id <= 0)
        {
            return;
        }

        if (_levelObjectsHolder.IslandsParent.TryGetComponent(out TrainigSequenceLevel1 trainigSequenceLevel))
        {
            trainigSequenceLevel.Initialize(_levelObjectsHolder, _buferIslandsHolder, _selectHandler);
            trainigSequenceLevel.StartTraining();
        }
    }
}
