using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class TrainigSequence : MonoBehaviour
{
    [SerializeField] private RectTransform _pointer;
    [SerializeField] private Image _pointerImage;

    protected LevelLoader LevelLoader { get; private set; }
    protected CustomizationWindow CustomizationMenu { get; private set; }
    protected LevelObjectsHolder LevelObjectsHolder { get; private set; }
    protected BuferIslandsHolder BuferIslandsHolder { get; private set; }
    protected SelectHandler SelectHandler { get; private set; }
    protected UnitMover UnitMover { get; private set; }
    protected Camera MainCamera { get; private set; }
    protected BoostButtonActivator BoostButtonActivator { get; private set; }
    protected LevelProgressTracker LevelProgressTracker { get; private set; }
    protected UIOrientationChanger UIOrientationChanger { get; private set; }
    protected GameProgressStorage ProgressStorage { get; private set; }
    protected MenuWindow InGameMenu { get; private set; }
    protected FinalScoreWindow FinalScoreWindow { get; private set; }
    protected MenuTrainigSequence MenuTrainigSequence { get; private set; }
    protected ScreenSizeChangeTracker ScreenSizeChangeTracker { get; private set; }
    protected RectTransform Pointer => _pointer;
    protected Image PointerImage => _pointerImage;
    protected float FadeDuration { get; private set; } = 0.55f;
    protected Canvas Canvas { get; private set; }

    private WaitForEndOfFrame _wait;


    public void Initialize(LevelObjectsHolder levelObjectsHolder, BuferIslandsHolder buferIslandsHolder,
                           SelectHandler selectHandler, UnitMover unitMover, Camera mainCamera,
                           BoostButtonActivator boostButtonActivator, LevelProgressTracker levelProgressTracker,
                           UIOrientationChanger uIOrientationChanger, GameProgressStorage progressStorage,
                           MenuWindow inGameMenu, FinalScoreWindow finalScoreWindow, MenuTrainigSequence menuTrainigSequence,
                           ScreenSizeChangeTracker screenSizeChangeTracker, Canvas canvas, LevelLoader levelLoader,
                           CustomizationWindow customizationMenu)
    {
        LevelObjectsHolder = levelObjectsHolder;
        BuferIslandsHolder = buferIslandsHolder;
        SelectHandler = selectHandler;
        UnitMover = unitMover;
        MainCamera = mainCamera;
        BoostButtonActivator = boostButtonActivator;
        LevelProgressTracker = levelProgressTracker;
        UIOrientationChanger = uIOrientationChanger;
        ProgressStorage = progressStorage;
        InGameMenu = inGameMenu;
        FinalScoreWindow = finalScoreWindow;
        MenuTrainigSequence = menuTrainigSequence;
        ScreenSizeChangeTracker = screenSizeChangeTracker;
        Canvas = canvas;
        LevelLoader = levelLoader;
        CustomizationMenu = customizationMenu;
        _wait = new WaitForEndOfFrame();
    }

    public void StartTrainingNextFrame()
    {
        StartCoroutine(StartingTrainingNextFrame());
    }

    public abstract void StartTraining();

    protected void ResetLevelState()
    {
        BoostButtonActivator.ActivateAllButtons();
        ActivateAllColliders();
    }

    protected void ActivatePointer()
    {
        DOTween.Sequence().Append(PointerImage.DOFade(1f, FadeDuration)
                              .SetEase(Ease.InOutQuad));
    }

    protected void DeactivatePointer()
    {
        DOTween.Sequence().Append(PointerImage.DOFade(0f, FadeDuration)
                              .SetEase(Ease.InOutQuad));
    }

    protected void ActivateAllColliders()
    {
        foreach (Island island in LevelObjectsHolder.Islands)
        {
            ActivateColliders(island);
        }

        ActivateColliders(BuferIslandsHolder.CurrentIsland);
    }

    protected void DeactivateColliders()
    {
        foreach (Island island in LevelObjectsHolder.Islands)
        {
            DeactivateColliders(island);
        }

        DeactivateColliders(BuferIslandsHolder.CurrentIsland);
    }

    protected void DeactivateColliders(BaseIsland island)
    {
        island.Collider.enabled = false;

        foreach (IslandPoint islandPoint in island.Points)
        {
            if (islandPoint.IsFree)
            {
                continue;
            }

            islandPoint.OccupiedUnit.Collider.enabled = false;
        }
    }

    protected void AddBost(BoostType boostType)
    {
        int boostAmount = ProgressStorage.GetBoostAmount(boostType);
        boostAmount++;
        ProgressStorage.SetBoostAmount(boostType, boostAmount);
    }

    protected void UpdatePointerPosition(RectTransform buttonRectTransform)
    {
        Vector3 offset = new Vector3(0, Pointer.rect.height * Canvas.scaleFactor, 0);
        Pointer.position = buttonRectTransform.position + offset;
        Pointer.localEulerAngles = buttonRectTransform.localEulerAngles;
    }

    private void ActivateColliders(BaseIsland island)
    {
        island.Collider.enabled = true;

        foreach (IslandPoint islandPoint in island.Points)
        {
            if (islandPoint.IsFree)
            {
                continue;
            }

            islandPoint.OccupiedUnit.Collider.enabled = true;
        }
    }

    private IEnumerator StartingTrainingNextFrame()
    {
        yield return _wait;

        StartTraining();
    }
}
