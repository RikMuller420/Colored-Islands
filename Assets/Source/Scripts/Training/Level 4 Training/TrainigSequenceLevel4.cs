using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TrainigSequenceLevel4 : TrainigSequence
{
    [SerializeField] private RectTransform _verticalOrientationPointerPosition;
    [SerializeField] private RectTransform _horizontalOrientationPointerPosition;
    [SerializeField] private CanvasGroup _angyBarBubble;
    [SerializeField] private TextMeshProUGUI _angryBarDescription;

    private float _startDelay = 1f;
    private float _hideAngyBarDelay = 6f;
    private float _bubbleFadeDuration = 0.7f;
    private float _pointerAppearDelay = 0.5f;
    private float _descriptionTypeDuration = 1.5f;
    private float _angryValueForStartTraining = 0.4f;
    private BoostButton _freezeObjectivesBoostButton;
    private bool _isTrainingStarted = false;
    private bool _isTrainingDone = false;

    public override void StartTraining()
    {
        BoostButtonActivator.DeactivateAllButtons();
        UIOrientationChanger.OrientationChanged += UpdatePointerPosition;
        InGameMenu.MenuOpened += OnMenuOpened;
        InGameMenu.MenuClosed += OnMenuClosed;

        BoostButtonActivator.ActivateButtonImmediate(BoostType.GrowBuferIsland);
        BoostButtonActivator.SetButtonNonInteractible(BoostType.GrowBuferIsland);
        BoostButtonActivator.ActivateButtonImmediate(BoostType.FinishIsland);
        BoostButtonActivator.SetButtonNonInteractible(BoostType.FinishIsland);

        _freezeObjectivesBoostButton = BoostButtonActivator.GetBoostButton(BoostType.FreezeObjectives);
        _freezeObjectivesBoostButton.TryBoostApplying += OnTryApplyingBoost;

        LevelProgressTracker.PauseTracking();
        LevelProgressTracker.AngryChanged += OnAngryValueChanged;
        StartCoroutine(OpenAngyBarHintInDelay());
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

        UpdatePointerPosition();
        ActivatePointer();
    }

    private IEnumerator OpenAngyBarHintInDelay()
    {
        yield return new WaitForSeconds(_startDelay);

        string description = _angryBarDescription.text;
        _angryBarDescription.text = "";
        DOTween.Sequence().Append(_angyBarBubble.DOFade(1f, _bubbleFadeDuration))
                          .Join(_angryBarDescription.DOText(description, _descriptionTypeDuration).SetEase(Ease.Linear));
        StartCoroutine(HideAngyBarInDelay());
    }

    private IEnumerator HideAngyBarInDelay()
    {
        yield return new WaitForSeconds(_hideAngyBarDelay);

        DOTween.Sequence().Append(_angyBarBubble.DOFade(0f, _bubbleFadeDuration));
        LevelProgressTracker.ContinueTracking();
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
        DeactivatePointer();
        _isTrainingDone = true;
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
