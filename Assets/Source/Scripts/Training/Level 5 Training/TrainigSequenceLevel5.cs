using System.Collections;
using UnityEngine;

public class TrainigSequenceLevel5 : TrainigSequence
{
    [SerializeField] private RectTransform _verticalOrientationPointerPosition;
    [SerializeField] private RectTransform _horizontalOrientationPointerPosition;

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

    public override void StartTraining()
    {
        BoostButtonActivator.DeactivateAllButtons();
        UIOrientationChanger.OrientationChanged += UpdatePointerPosition;
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
    }

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
}
