using UnityEngine;
using UnityEngine.UI;

public class LevelRewardWindow : MenuWindow
{
    private const string RewardedAddId = "DoubleLevelReward";

    [SerializeField] private LevelRewardView _levelRewardView;
    [SerializeField] private AddMultipliedRewardWindow _addMultipliedRewardWindow;

    [SerializeField] private Button _receiveButton;
    [SerializeField] private Button _receiveWithAdsButton;

    private UnitsHatSettings _unitsHatSettings;
    private GameProgressStorage _progressStorage;
    private RewardedAdProvider _rewardedAdProvider;
    private UpgradesProvider _upgradesProvider;
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

    public void Initialize(UnitsHatSettings unitsHatSettings, GameProgressStorage progressStorage,
                            RewardedAdProvider rewardedAdProvider, UpgradesProvider upgradesProvider)
    {
        _levelRewardView.Initialize(unitsHatSettings, upgradesProvider);
        _levelRewardSaver = new LevelRewardSaver(progressStorage, upgradesProvider);
        _unitsHatSettings = unitsHatSettings;
        _progressStorage = progressStorage;
        _rewardedAdProvider = rewardedAdProvider;
        _upgradesProvider = upgradesProvider;
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
