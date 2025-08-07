using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TrainigSequenceLevel5 : TrainigSequence
{
    [SerializeField] private RectTransform _verticalOrientationPointerPosition;
    [SerializeField] private RectTransform _horizontalOrientationPointerPosition;
    [SerializeField] private CanvasGroup _finishTrainingPanel;
    [SerializeField] private Button _lastStepButton;
    [SerializeField] private Image _fullDimImage;

    private float _waitTime = 0.7f;
    private WaitForSeconds _wait;
    private BoostButton _reducePaintsBoostButton;
    private bool _isTrainingDone = false;
    private int _movesBeforeTraining = 2;
    private int _performedMoves = 0;
    private bool _isTrainingStarted = false;

    private void Awake()
    {
        _wait = new WaitForSeconds(_waitTime);
    }

    private void OnDestroy()
    {
        ScreenSizeChangeTracker.ScreenSizeChanged -= OnScreenSizeChanged;
        InGameMenu.MenuOpened -= OnMenuOpened;
        InGameMenu.MenuClosed -= OnMenuClosed;
        UnitMover.UnitsMoved -= OnUnitsMoved;
        _reducePaintsBoostButton.TryBoostApplying -= OnTryApplyingReducePaintsBoost;
        FinalScoreWindow.ScoreShowed -= OnFinalScoreShowed;
    }

    public override void StartTraining()
    {
        BoostButtonActivator.DeactivateAllButtons();
        ScreenSizeChangeTracker.ScreenSizeChanged += OnScreenSizeChanged;
        InGameMenu.MenuOpened += OnMenuOpened;
        InGameMenu.MenuClosed += OnMenuClosed;
        UnitMover.UnitsMoved += OnUnitsMoved;

        BoostButtonActivator.ActivateButtonImmediate(BoostType.GrowBuferIsland);
        BoostButtonActivator.ActivateButtonImmediate(BoostType.FinishIsland);
        BoostButtonActivator.ActivateButtonImmediate(BoostType.FreezeObjectives);

        BoostButtonActivator.SetButtonNonInteractible(BoostType.GrowBuferIsland);
        BoostButtonActivator.SetButtonNonInteractible(BoostType.FinishIsland);
        BoostButtonActivator.SetButtonNonInteractible(BoostType.FreezeObjectives);

        _reducePaintsBoostButton = BoostButtonActivator.GetBoostButton(BoostType.ReducePaints);
        _reducePaintsBoostButton.TryBoostApplying += OnTryApplyingReducePaintsBoost;

        FinalScoreWindow.ScoreShowed += OnFinalScoreShowed;
        _lastStepButton.onClick.AddListener(GoToMenuTraining);
    }

    private void OnScreenSizeChanged(Vector2 _) => UpdatePointerPosition();

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
        DeactivateColliders();
        LevelProgressTracker.PauseTracking();
        BoostButtonActivator.ActivateButtonWithFade(BoostType.ReducePaints);

        yield return _wait;

        UpdatePointerPosition();
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

    private void UpdatePointerPosition()
    {
        RectTransform targetPosition = UIOrientationChanger.IsVertical
            ? _verticalOrientationPointerPosition
            : _horizontalOrientationPointerPosition;

        Pointer.position = targetPosition.position;
        Pointer.localEulerAngles = targetPosition.localEulerAngles;
    }

    private void OnFinalScoreShowed()
    {
        if (ProgressStorage.IsTrainingFinished)
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
        ProgressStorage.SetSpinCount(ProgressStorage.AviableSpinCount + 1);

        ProgressStorage.SetTrainingFinished(true);
        ProgressStorage.Save();
    }

    private void GoToMenuTraining()
    {
        DOTween.Sequence().Append(_fullDimImage.DOFade(1f, FadeDuration)
                          .SetEase(Ease.InOutQuad))
                          .OnComplete(() => MenuTrainigSequence.StartTraining());
    }
}
