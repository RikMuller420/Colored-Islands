using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Menu.Boosts;
using TMPro;
using UnityEngine;

namespace SlimeGround.Gameplay.Training
{
	public class TrainigSequenceLevel4 : TrainigSequence
	{
	    [SerializeField] private CanvasGroup _angyBarBubble;
	    [SerializeField] private TextMeshProUGUI _angryBarDescription;

	    private float _startDelay = 1f;
	    private float _bubbleFadeDuration = 0.7f;
	    private float _pointerAppearDelay = 0.5f;
	    private float _descriptionTypeDuration = 1.5f;
	    private float _angryValueForStartTraining = 0.4f;
	    private BoostButton _freezeObjectivesBoostButton;
		private bool _isEventsSubscribed = false;
		private bool _isTrainingStarted = false;
	    private bool _isTrainingDone = false;
	    private List<BaseIsland> _finishedIslands = new();
	    private RectTransform _buttonRectTransform;
	    private bool _isHintHided = false;

	    private void OnDestroy()
	    {
			if (_isEventsSubscribed)
			{
				ScreenSizeChangeTracker.ScreenSizeChanged -= OnScreenSizeChanged;
				InGameMenu.MenuOpened -= OnMenuOpened;
				InGameMenu.MenuClosed -= OnMenuClosed;
				_freezeObjectivesBoostButton.TryBoostApplying -= OnTryApplyingBoost;
				LevelProgressTracker.AngryChanged -= OnAngryValueChanged;
				LevelProgressTracker.FirstMoveDone -= OnUnitSelected;
				LevelProgressTracker.IslandFinished -= OnIslandFinished;

				_isEventsSubscribed = false;
			}

	        ResetLevelState();
	    }

	    public override void StartTraining()
	    {
			if (_isEventsSubscribed == false)
			{
				BoostButtonActivator.DeactivateAllButtons();
				ScreenSizeChangeTracker.ScreenSizeChanged += OnScreenSizeChanged;
				InGameMenu.MenuOpened += OnMenuOpened;
				InGameMenu.MenuClosed += OnMenuClosed;
				_freezeObjectivesBoostButton.TryBoostApplying += OnTryApplyingBoost;
				LevelProgressTracker.AngryChanged += OnAngryValueChanged;
				UnitsSelectedEvent.UnitsSelected += OnUnitSelected;
				LevelProgressTracker.IslandFinished += OnIslandFinished;

				_isEventsSubscribed = true;
			}

			BoostButtonActivator.ActivateButtonImmediate(BoostType.GrowBuferIsland);
	        BoostButtonActivator.SetButtonNonInteractible(BoostType.GrowBuferIsland);
	        BoostButtonActivator.ActivateButtonImmediate(BoostType.FinishIsland);
	        BoostButtonActivator.SetButtonNonInteractible(BoostType.FinishIsland);

	        _freezeObjectivesBoostButton = BoostButtonActivator.GetBoostButton(BoostType.FreezeObjectives);
	        _buttonRectTransform = BoostButtonActivator.GetBoostButtonRectTransform(BoostType.FreezeObjectives);

	        StartCoroutine(OpenAngyBarHintInDelay());
	    }

	    private void OnScreenSizeChanged(Vector2 _) => UpdatePointerPosition(_buttonRectTransform);

	    private void OnIslandFinished(BaseIsland island)
	    {
	        _finishedIslands.Add(island);
	    }

	    private void OnUnitSelected()
	    {
	        if (_isHintHided)
	        {
	            return;
	        }

	        DOTween.Sequence().Append(_angyBarBubble.DOFade(0f, _bubbleFadeDuration));
	        _isHintHided = true;
	    }

	    private void OnAngryValueChanged(float value)
	    {
	        if (_isTrainingStarted == false)
	        {
	            if (value > _angryValueForStartTraining)
	            {
	                StartCoroutine(StartBoostTraining());
	                _isTrainingStarted = true;
	            }
	        }
	    }

	    private IEnumerator StartBoostTraining()
	    {
	        LevelProgressTracker.PauseTracking();
	        DeactivateAllColliders();

	        BoostButtonActivator.ActivateButtonWithFade(BoostType.FreezeObjectives);

	        yield return new WaitForSeconds(_pointerAppearDelay);

	        UpdatePointerPosition(_buttonRectTransform);
	        ActivatePointer();
	    }

	    private IEnumerator OpenAngyBarHintInDelay()
	    {
	        yield return new WaitForSeconds(_startDelay);

	        string description = _angryBarDescription.text;
	        _angryBarDescription.text = "";
	        DOTween.Sequence().Append(_angyBarBubble.DOFade(1f, _bubbleFadeDuration))
	                          .Join(_angryBarDescription.DOText(description, _descriptionTypeDuration).SetEase(Ease.Linear));
	    }

	    private void OnMenuOpened()
	    {
	        if (_isTrainingDone == false)
	        {
	            Pointer.gameObject.SetActive(false);
	        }
	    }

	    private void OnMenuClosed()
	    {
	        if (_isTrainingDone == false)
	        {
	            Pointer.gameObject.SetActive(true);
	        }
	    }

	    private void OnTryApplyingBoost()
	    {
	        AddBost(BoostType.FreezeObjectives);
	        ActivateAllColliders();

	        foreach (BaseIsland island in _finishedIslands)
	        {
	            DeactivateColliders(island);
	        }

	        DeactivatePointer();
	        _isTrainingDone = true;
	    }
	}
}
