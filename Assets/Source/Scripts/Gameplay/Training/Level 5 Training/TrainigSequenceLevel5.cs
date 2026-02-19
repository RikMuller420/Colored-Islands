using System.Collections;
using DG.Tweening;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Gameplay.Units;
using SlimeGround.Menu.Boosts;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Gameplay.Training
{
	public class TrainigSequenceLevel5 : TrainigSequence
	{
	    [SerializeField] private CanvasGroup _finishTrainingPanel;
	    [SerializeField] private Button _lastStepButton;
	    [SerializeField] private Image _fullDimImage;

	    private float _waitTime = 0.7f;
	    private WaitForSeconds _wait;
	    private BoostButton _reducePaintsBoostButton;
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
				ScreenSizeChangeTracker.ScreenSizeChanged -= OnScreenSizeChanged;
				InGameMenu.MenuOpened -= OnMenuOpened;
				InGameMenu.MenuClosed -= OnMenuClosed;
				UnitMovedEvent.UnitsMoved -= OnUnitsMoved;
				_reducePaintsBoostButton.TryBoostApplying -= OnTryApplyingReducePaintsBoost;
				FinalScoreWindow.ScoreShowed -= OnFinalScoreShowed;
				_lastStepButton.onClick.RemoveListener(GoToMenuTraining);

				_isEventsSubscribed = false;
			}

	        ResetLevelState();
	    }

	    public override void StartTraining()
	    {
	        BoostButtonActivator.DeactivateAllButtons();

			BoostButtonActivator.ActivateButtonImmediate(BoostType.GrowBuferIsland);
	        BoostButtonActivator.ActivateButtonImmediate(BoostType.FinishIsland);
	        BoostButtonActivator.ActivateButtonImmediate(BoostType.FreezeObjectives);

	        BoostButtonActivator.SetButtonNonInteractible(BoostType.GrowBuferIsland);
	        BoostButtonActivator.SetButtonNonInteractible(BoostType.FinishIsland);
	        BoostButtonActivator.SetButtonNonInteractible(BoostType.FreezeObjectives);

	        _reducePaintsBoostButton = BoostButtonActivator.GetBoostButton(BoostType.ReducePaints);
	        _buttonRectTransform = BoostButtonActivator.GetBoostButtonRectTransform(BoostType.ReducePaints);

			if (_isEventsSubscribed == false)
			{
				ScreenSizeChangeTracker.ScreenSizeChanged += OnScreenSizeChanged;
				InGameMenu.MenuOpened += OnMenuOpened;
				InGameMenu.MenuClosed += OnMenuClosed;
				UnitMovedEvent.UnitsMoved += OnUnitsMoved;
				_reducePaintsBoostButton.TryBoostApplying += OnTryApplyingReducePaintsBoost;
				FinalScoreWindow.ScoreShowed += OnFinalScoreShowed;
				_lastStepButton.onClick.AddListener(GoToMenuTraining);

				_isEventsSubscribed = true;
			}
	    }

	    private void OnScreenSizeChanged(Vector2 _) => UpdatePointerPosition(_buttonRectTransform);

	    private void OnUnitsMoved(UnitsMoveInfo _)
	    {
	        if (_isTrainingStarted == false)
	        {
	            _performedMoves++;

	            if (_performedMoves == _movesBeforeTraining)
	            {
	                StartCoroutine(StartTrainingMove());
	                _isTrainingStarted = true;
	            }
	        }
	    }

	    private IEnumerator StartTrainingMove()
	    {
	        DeactivateAllColliders();
	        LevelProgressTracker.PauseTracking();
	        BoostButtonActivator.ActivateButtonWithFade(BoostType.ReducePaints);

	        yield return _wait;

	        UpdatePointerPosition(_buttonRectTransform);
	        ActivatePointer();
	    }

	    private void OnTryApplyingReducePaintsBoost()
	    {
	        AddBost(BoostType.ReducePaints);
	        DeactivatePointer();
	        ActivateAllColliders();
	        LevelProgressTracker.ContinueTracking();

	        _isTrainingDone = true;
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

	    private void OnFinalScoreShowed()
	    {
	        if (PlayerData.IsTrainingFinished)
	        {
	            return;
	        }

	        _finishTrainingPanel.blocksRaycasts = true;
	        DOTween.Sequence().Append(_finishTrainingPanel.DOFade(1f, FadeDuration)
	                          .SetEase(Ease.InOutQuad))
	                          .OnComplete(() => _lastStepButton.interactable = true);

	        AddBost(BoostType.GrowBuferIsland);
	        AddBost(BoostType.FinishIsland);
	        AddBost(BoostType.FreezeObjectives);
	        AddBost(BoostType.ReducePaints);
	        PlayerData.SetSpinCount(PlayerData.AviableSpinCount + 1);

	        PlayerData.SetTrainingFinished();
	        PlayerData.Save();
	    }

	    private void GoToMenuTraining()
	    {
	        DOTween.Sequence().Append(_fullDimImage.DOFade(1f, FadeDuration)
	                          .SetEase(Ease.InOutQuad))
	                          .OnComplete(() => MenuTrainigSequence.StartTraining());
	    }
	}
}
