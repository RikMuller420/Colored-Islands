using System.Collections;
using SlimeGround.Data.Saves;
using SlimeGround.Integration.Metrics;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Roulette
{
	public class Roulette : MonoBehaviour
	{
	    private const string SpinButtonEnableBool = "IsEnable";

	    [SerializeField] private Button _spinButton;
	    [SerializeField] private Animator _spinButtonAnimator;
	    [SerializeField] private RouletteWheel _rouletteWheel;
	    [SerializeField] private RouletteRewardWindow _rouletteRewardWindow;

	    private bool _isPrepared = false;
	    private float _rewardWindowDelay = 0.6f;
	    private WaitForSeconds _wait;

	    private PlayerDataProvider _playerDataProvider;

	    private void Awake()
	    {
	        _wait = new WaitForSeconds(_rewardWindowDelay);
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

	    public void Initialize(PlayerDataProvider playerDataProvider)
	    {
	        _playerDataProvider = playerDataProvider;
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
	        _spinButtonAnimator.SetBool(SpinButtonEnableBool, false);

	        int newSpinCount = _playerDataProvider.AviableSpinCount - 1;
	        _playerDataProvider.SetSpinCount(newSpinCount);
	        _playerDataProvider.Save();
	        MetricSaver.SpinRoulete();
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
	        _spinButtonAnimator.SetBool(SpinButtonEnableBool, true);
	    }
	}
}
