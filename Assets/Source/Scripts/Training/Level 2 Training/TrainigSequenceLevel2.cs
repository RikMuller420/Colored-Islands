using System.Collections;
using UnityEngine;

public class TrainigSequenceLevel2 : TrainigSequence
{
    [SerializeField] private RectTransform _verticalOrientationPointerPosition;
    [SerializeField] private RectTransform _horizontalOrientationPointerPosition;

    private float _startDelay = 0.7f;
    private WaitForSeconds _startWait;
    private BoostButton _boostButton;
    private bool _isTrainingDone = false;

    private void Awake()
    {
        _startWait = new WaitForSeconds(_startDelay);
    }

    public override void StartTraining()
    {
        BoostButtonActivator.DeactivateAllButtons();
        DeactivateAllColliders();
        UIOrientationChanger.OrientationChanged += UpdatePointerPosition;
        InGameMenu.MenuOpened += OnMenuOpened;
        InGameMenu.MenuClosed += OnMenuClosed;
        _boostButton = BoostButtonActivator.GetBoostButton(BoostType.GrowBuferIsland);
        _boostButton.TryBoostApplying += OnTryApplyingBoost;
        
        StartCoroutine(StartFirstTrainingMove());
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

    private IEnumerator StartFirstTrainingMove()
    {
        yield return _startWait;

        BoostButtonActivator.ActivateButtonWithFade(BoostType.GrowBuferIsland);
        UpdatePointerPosition();
        ActivatePointer();
    }

    private void UpdatePointerPosition()
    {
        RectTransform targetPosition = UIOrientationChanger.IsVertical
            ? _verticalOrientationPointerPosition
            : _horizontalOrientationPointerPosition;

        Pointer.position = targetPosition.position;
        Pointer.localEulerAngles = targetPosition.localEulerAngles;
    }

    private void OnTryApplyingBoost()
    {
        int boostAmount = ProgressStorage.GetBoostAmount(BoostType.GrowBuferIsland);
        boostAmount++;
        ProgressStorage.SetBoostAmount(BoostType.GrowBuferIsland, boostAmount);
        ActivateAllColliders();
        DeactivatePointer();
        _isTrainingDone = true;
    }
}
