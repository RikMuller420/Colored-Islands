using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    [SerializeField] private Button _spinButton;
    [SerializeField] private RouletteWheel _rouletteWheel;
    [SerializeField] private RouletteRewardWindow _rouletteRewardWindow;

    private bool _isPrepared = false;
    private float _rewerdWindowDelay = 0.6f;
    private WaitForSeconds _wait;

    private GameProgressStorage _progressStorage;

    private void Awake()
    {
        _wait = new WaitForSeconds(_rewerdWindowDelay);
    }

    private void OnEnable()
    {
        _spinButton.onClick.AddListener(Spin);
        _rouletteWheel.SpinFinished += OnSpinFinished;
        _rouletteRewardWindow.MenuClosed += OnRewardReceived;
    }

    private void OnDisable()
    {
        _spinButton.onClick.RemoveListener(Spin);
        _rouletteWheel.SpinFinished -= OnSpinFinished;
        _rouletteRewardWindow.MenuClosed -= OnRewardReceived;
    }

    public void Initialize(GameProgressStorage progressStorage)
    {
        _progressStorage = progressStorage;
    }

    public void PrepareRoulette()
    {
        if (_isPrepared == false)
        {
            _rouletteWheel.PrepareSlots();
            _isPrepared = true;
        }
    }

    private void Spin()
    {
        _rouletteWheel.Spin();
        _isPrepared = false;
        _spinButton.interactable = false;

        int newSpinCount = _progressStorage.AviableSpinCount - 1;
        _progressStorage.SetSpinCount(newSpinCount);
    }

    private void OnSpinFinished(Slot winnedSlot)
    {
        StartCoroutine(OpenRewardWindowInDelay(winnedSlot));
    }

    private IEnumerator OpenRewardWindowInDelay(Slot winnedSlot)
    {
        yield return _wait;

        _rouletteRewardWindow.Open(winnedSlot);
    }

    private void OnRewardReceived()
    {
        PrepareRoulette();
        _spinButton.interactable = true;

        //Закрыть окно, если спинов больше нет
    }
}
