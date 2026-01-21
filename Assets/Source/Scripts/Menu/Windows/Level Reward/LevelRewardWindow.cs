using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Data.ScriptableObjects.LevelRewards;
using SlimeGround.Integration.Ads;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Extensions.Windows;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.LevelReward
{
	public class LevelRewardWindow : MenuWindow
	{
	    private const string RewardedAddId = "DoubleLevelReward";

	    [SerializeField] private PlayerDataProvider _playerData;
	    [SerializeField] private UnitsHatSettings _hatSettings;
	    [SerializeField] private LevelRewardView _levelRewardView;
	    [SerializeField] private AddMultipliedRewardWindow _addMultipliedRewardWindow;

	    [SerializeField] private Button _receiveButton;
	    [SerializeField] private Button _receiveWithAdsButton;

	    private RewardedAdProvider _rewardedAdProvider;
	    private LevelRewardSaver _levelRewardSaver;

	    private LevelRewardData _currentReward;
	    private int _adsMultiplier = 2;

	    protected override void OnEnable()
	    {
	        _receiveButton.onClick.AddListener(ReceiveReward);
	        _receiveWithAdsButton.onClick.AddListener(TryReceiveRewardWithAdd);
	    }

	    protected override void OnDisable()
	    {
	        _receiveButton.onClick.RemoveListener(ReceiveReward);
	        _receiveWithAdsButton.onClick.RemoveListener(TryReceiveRewardWithAdd);
	    }

	    public void Initialize(RewardedAdProvider rewardedAdProvider, UpgradesProvider upgradesProvider)
	    {
	        _levelRewardView.Initialize(_hatSettings, upgradesProvider);
	        _levelRewardSaver = new LevelRewardSaver(_playerData, upgradesProvider);
	        _rewardedAdProvider = rewardedAdProvider;

	        _addMultipliedRewardWindow.Initialize(_hatSettings, upgradesProvider);
	    }

	    public void Open(LevelRewardData levelRewardData)
	    {
	        _currentReward = levelRewardData;
	        _levelRewardView.SetIcons(levelRewardData);

	        OpenUnclosableWindow();
	    }

	    private void ReceiveReward()
	    {
	        _levelRewardSaver.AddReward(_currentReward);
	        Close();
	        MetricSaver.ReceiveStandartLevelReward();
	    }

	    private void TryReceiveRewardWithAdd()
	    {
	        _rewardedAdProvider.ShowAdvReward(RewardedAddId, ReceiveRewardWithAdd);
	        MetricSaver.ReceiveMultiplayedLevelRewardWithAdd();
	    }

	    private void ReceiveRewardWithAdd()
	    {
	        _levelRewardSaver.AddReward(_currentReward, _adsMultiplier);
	        Close();
	        _addMultipliedRewardWindow.Open(_currentReward, _adsMultiplier);
	    }
	}
}
