using System.Collections;
using DG.Tweening;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Units;
using SlimeGround.Menu.Boosts;
using SlimeGround.Menu.Extensions.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Gameplay.Training
{
	public class TrainigSequenceLevel3 : TrainigSequence
	{
	    [SerializeField] private MenuWindow _customizationHint;
	    [SerializeField] private Button _openCustomizationButton;

	    [SerializeField] private Island _islandForBoost;
	    [SerializeField] private RectTransform _worldSpacePointer;
	    [SerializeField] private Image _worldSpacePointerImage;

	    private float _waitTime = 0.7f;
	    private WaitForSeconds _wait;
	    private BoostButton _finishIslandBoostButton;
	    private bool _isTrainingDone = false;
	    private int _movesBeforeTraining = 2;
	    private int _performedMoves = 0;
		private bool _isEventsSubscribed = false;
		private bool _isTrainingStarted = false;
	    private RectTransform _buttonRectTransform;

	    private void Awake()
	    {
	        _wait = new WaitForSeconds(_waitTime);
	    }

	    private void OnDestroy()
	    {
			if (_isEventsSubscribed)
			{
				InGameMenu.MenuOpened -= OnMenuOpened;
				InGameMenu.MenuClosed -= OnMenuClosed;
				UnitMovedEvent.UnitsMoved -= OnUnitsMoved;
				ScreenSizeChangeTracker.ScreenSizeChanged -= OnScreenSizeChanged;

				_islandForBoost.IslandFinished -= OnIslandForBoostFinished;
				_finishIslandBoostButton.TryBoostApplying -= OnTryApplyingFinishIslandBoost;
				_openCustomizationButton.onClick.RemoveListener(OpenCustomizationMenu);

				_isEventsSubscribed = false;
			}

	        ResetLevelState();
	    }

	    public override void StartTraining()
	    {
	        BoostButtonActivator.ActivateButtonImmediate(BoostType.GrowBuferIsland);
	        BoostButtonActivator.SetButtonNonInteractible(BoostType.GrowBuferIsland);

	        _finishIslandBoostButton = BoostButtonActivator.GetBoostButton(BoostType.FinishIsland);
	        _buttonRectTransform = BoostButtonActivator.GetBoostButtonRectTransform(BoostType.FinishIsland);

			if (_isEventsSubscribed == false)
			{
				BoostButtonActivator.DeactivateAllButtons();
				InGameMenu.MenuOpened += OnMenuOpened;
				InGameMenu.MenuClosed += OnMenuClosed;
				UnitMovedEvent.UnitsMoved += OnUnitsMoved;
				ScreenSizeChangeTracker.ScreenSizeChanged += OnScreenSizeChanged;
				_finishIslandBoostButton.TryBoostApplying += OnTryApplyingFinishIslandBoost;
				_islandForBoost.IslandFinished += OnIslandForBoostFinished;
				_openCustomizationButton.onClick.AddListener(OpenCustomizationMenu);

				_isEventsSubscribed = true;
			}

			if (PlayerData.IsCustomizationWindowWasOpened == false)
			{
				_customizationHint.Open();
			}
		}

	    private void OpenCustomizationMenu()
	    {
	        LevelLoader.LoadMainMenu();
	        CustomizationMenu.Open();
	    }
	        
	    private void OnScreenSizeChanged(Vector2 _) => UpdatePointerPosition(_buttonRectTransform);

	    private void OnUnitsMoved(UnitsMoveInfo _)
	    {
	        if (_isTrainingStarted == false)
	        {
	            _performedMoves++;

	            if (_performedMoves == _movesBeforeTraining)
	            {
	                StartCoroutine(StartFirstTrainingMove());
	                _isTrainingStarted = true;
	            }
	        }
	    }

	    private IEnumerator StartFirstTrainingMove()
	    {
	        DeactivateAllColliders();
	        BoostButtonActivator.ActivateButtonWithFade(BoostType.FinishIsland);
			GameplayDimmer.Activate();

	        yield return _wait;

	        UpdatePointerPosition(_buttonRectTransform);
	        ActivatePointer();
	    }

	    private void OnTryApplyingFinishIslandBoost()
	    {
	        AddBost(BoostType.FinishIsland);
	        DeactivatePointer();
	        BoostButtonActivator.SetButtonNonInteractible(BoostType.FinishIsland);
	        _islandForBoost.Collider.enabled = true;
	        ActivateIslandPointer();
			GameplayDimmer.Deactivate();
		}

		private void OnIslandForBoostFinished(Island island)
	    {
	        ActivateAllColliders();
	        DeactivateColliders(island);
	        DOTween.Sequence().Append(_worldSpacePointerImage.DOFade(0f, FadeDuration)
	                                  .SetEase(Ease.InOutQuad));
	        _isTrainingDone = true;
	    }

	    private void ActivateIslandPointer()
	    {
	        _worldSpacePointer.gameObject.SetActive(true);
	        DOTween.Sequence().Append(_worldSpacePointerImage.DOFade(1f, FadeDuration)
	                          .SetEase(Ease.InOutQuad));
	        _worldSpacePointer.LookAt(MainCamera.transform);
	    }

	    private void OnMenuOpened()
	    {
	        if (_isTrainingDone == false)
	        {
	            Pointer.gameObject.SetActive(false);
	            _worldSpacePointer.gameObject.SetActive(false);
	        }
	    }

	    private void OnMenuClosed()
	    {
	        if (_isTrainingDone == false)
	        {
	            Pointer.gameObject.SetActive(true);
	            _worldSpacePointer.gameObject.SetActive(true);
	        }
	    }
	}
}
