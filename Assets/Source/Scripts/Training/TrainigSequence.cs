using UnityEngine;

public abstract class TrainigSequence : MonoBehaviour
{
    protected LevelObjectsHolder LevelObjectsHolder { get; private set; }
    protected BuferIslandsHolder BuferIslandsHolder { get; private set; }
    protected SelectHandler SelectHandler { get; private set; }
    protected UnitMover UnitMover { get; private set; }
    protected Camera MainCamera { get; private set; }
    protected BoostButtonActivator BoostButtonActivator { get; private set; }
    protected LevelProgressTracker LevelProgressTracker { get; private set; }
    protected float FadeDuration { get; private set; } = 0.7f;


    public void Initialize(LevelObjectsHolder levelObjectsHolder, BuferIslandsHolder buferIslandsHolder,
                           SelectHandler selectHandler, UnitMover unitMover, Camera mainCamera,
                           BoostButtonActivator boostButtonActivator, LevelProgressTracker levelProgressTracker)
    {
        LevelObjectsHolder = levelObjectsHolder;
        BuferIslandsHolder = buferIslandsHolder;
        SelectHandler = selectHandler;
        UnitMover = unitMover;
        MainCamera = mainCamera;
        BoostButtonActivator = boostButtonActivator;
        LevelProgressTracker = levelProgressTracker;
    }

    public abstract void StartTraining();
}
