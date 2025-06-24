using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class TrainigSequence : MonoBehaviour
{
    [SerializeField] private RectTransform _pointer;
    [SerializeField] private Image _pointerImage;

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
    protected RectTransform Pointer => _pointer;
    protected Image PointerImage => _pointerImage;
    protected float FadeDuration { get; private set; } = 0.55f;

    private void OnDestroy()
    {
        BoostButtonActivator.ActivateAllButtons();
    }

    public void Initialize(LevelObjectsHolder levelObjectsHolder, BuferIslandsHolder buferIslandsHolder,
                           SelectHandler selectHandler, UnitMover unitMover, Camera mainCamera,
                           BoostButtonActivator boostButtonActivator, LevelProgressTracker levelProgressTracker,
                           UIOrientationChanger uIOrientationChanger, GameProgressStorage progressStorage,
                           MenuWindow inGameMenu)
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
    }

    public abstract void StartTraining();

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

    protected void DeactivateAllColliders()
    {
        foreach (Island island in LevelObjectsHolder.Islands)
        {
            DeactivateColliders(island);
        }

        DeactivateColliders(BuferIslandsHolder.CurrentIsland);
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

    private void DeactivateColliders(BaseIsland island)
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
}
