using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TrainigSequenceLevel3 : TrainigSequence
{
    [SerializeField] private Island _islandForBoost;
    [SerializeField] private RectTransform _verticalOrientationPointerPosition;
    [SerializeField] private RectTransform _horizontalOrientationPointerPosition;

    [SerializeField] private RectTransform _worldSpacePointer;
    [SerializeField] private Image _worldSpacePointerImage;

    private float _waitTime = 0.7f;
    private WaitForSeconds _wait;
    private BoostButton _finishIslandBoostButton;
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
        _islandForBoost.IslandFinished += OnIslandFinished;

        BoostButtonActivator.ActivateButtonImmediate(BoostType.GrowBuferIsland);
        BoostButtonActivator.SetButtonNonInteractible(BoostType.GrowBuferIsland);

        _finishIslandBoostButton = BoostButtonActivator.GetBoostButton(BoostType.FinishIsland);
        _finishIslandBoostButton.TryBoostApplying += OnTryApplyingFinishIslandBoost;
    }

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

        yield return _wait;

        UpdatePointerPosition();
        ActivatePointer();
    }

    private void OnTryApplyingFinishIslandBoost()
    {
        AddBost(BoostType.FinishIsland);
        DeactivatePointer();
        BoostButtonActivator.SetButtonNonInteractible(BoostType.FinishIsland);
        _islandForBoost.Collider.enabled = true;
        ActivateIslandPointer();
    }

    private void OnIslandFinished(Island _)
    {
        ActivateAllColliders();
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

    private void UpdatePointerPosition()
    {
        RectTransform targetPosition = UIOrientationChanger.IsVertical
            ? _verticalOrientationPointerPosition
            : _horizontalOrientationPointerPosition;

        Pointer.position = targetPosition.position;
        Pointer.localEulerAngles = targetPosition.localEulerAngles;
    }
}
