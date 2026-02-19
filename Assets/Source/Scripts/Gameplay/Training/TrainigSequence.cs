using System.Collections;
using DG.Tweening;
using SlimeGround.Core;
using SlimeGround.Core.InputHandling;
using SlimeGround.Data.Saves;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Gameplay.Units;
using SlimeGround.Menu.Extensions.Windows;
using SlimeGround.Menu.OrientationChanger;
using SlimeGround.Menu.Windows.Customization;
using SlimeGround.Menu.Windows.FinalScore;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Gameplay.Training
{
	public abstract class TrainigSequence : MonoBehaviour
	{
	    [SerializeField] private RectTransform _pointer;
	    [SerializeField] private Image _pointerImage;

		private WaitForEndOfFrame _wait;

		protected LevelLoader LevelLoader { get; private set; }
	    protected CustomizationWindow CustomizationMenu { get; private set; }
	    protected ILevelData CurrentLevelData { get; private set; }
	    protected BuferIslandsHolder BuferIslandsHolder { get; private set; }
	    protected IUnitsSelectedEvent UnitsSelectedEvent { get; private set; }
	    protected IUnitMovedEvent UnitMovedEvent { get; private set; }
	    protected Camera MainCamera { get; private set; }
	    protected BoostButtonActivator BoostButtonActivator { get; private set; }
	    protected LevelProgressTracker LevelProgressTracker { get; private set; }
	    protected UIOrientationChanger UIOrientationChanger { get; private set; }
	    protected PlayerDataProvider PlayerData { get; private set; }
	    protected MenuWindow InGameMenu { get; private set; }
	    protected FinalScoreWindow FinalScoreWindow { get; private set; }
	    protected MenuTrainigSequence MenuTrainigSequence { get; private set; }
	    protected ScreenSizeChangeTracker ScreenSizeChangeTracker { get; private set; }
	    protected RectTransform Pointer => _pointer;
	    protected Image PointerImage => _pointerImage;
	    protected float FadeDuration { get; private set; } = 0.55f;
	    protected Canvas Canvas { get; private set; }

	    public void Initialize(ILevelData currentLevelData, BuferIslandsHolder buferIslandsHolder,
	                           IUnitsSelectedEvent unitsSelectedEvent, IUnitMovedEvent unitMovedEvent, Camera mainCamera,
	                           BoostButtonActivator boostButtonActivator, LevelProgressTracker levelProgressTracker,
	                           UIOrientationChanger uIOrientationChanger, PlayerDataProvider playerData,
	                           MenuWindow inGameMenu, FinalScoreWindow finalScoreWindow, MenuTrainigSequence menuTrainigSequence,
	                           ScreenSizeChangeTracker screenSizeChangeTracker, Canvas canvas, LevelLoader levelLoader,
	                           CustomizationWindow customizationMenu)
	    {
	        CurrentLevelData = currentLevelData;
	        BuferIslandsHolder = buferIslandsHolder;
	        UnitsSelectedEvent = unitsSelectedEvent;
	        UnitMovedEvent = unitMovedEvent;
	        MainCamera = mainCamera;
	        BoostButtonActivator = boostButtonActivator;
	        LevelProgressTracker = levelProgressTracker;
	        UIOrientationChanger = uIOrientationChanger;
	        PlayerData = playerData;
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
	        foreach (Island island in CurrentLevelData.Islands)
	        {
	            ActivateColliders(island);
	        }

	        ActivateColliders(BuferIslandsHolder.CurrentIsland);
	    }

	    protected void DeactivateAllColliders()
	    {
	        foreach (Island island in CurrentLevelData.Islands)
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

				islandPoint.OccupiedUnit.Deactivate();
	        }
	    }

	    protected void AddBost(BoostType boostType)
	    {
	        int boostAmount = PlayerData.GetBoostAmount(boostType);
	        boostAmount++;
	        PlayerData.SetBoostAmount(boostType, boostAmount);
	        PlayerData.Save();
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

	            islandPoint.OccupiedUnit.Activate();
	        }
	    }

	    private IEnumerator StartingTrainingNextFrame()
	    {
	        yield return _wait;

	        StartTraining();
	    }
	}
}
