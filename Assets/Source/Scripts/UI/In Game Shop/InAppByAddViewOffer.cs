using UnityEngine;
using UnityEngine.UI;

public class InAppByAddViewOffer : MonoBehaviour
{
    [SerializeField] private InAppType _inAppType;
    [SerializeField] private Button _button;
    [SerializeField] private UpgradeIndicator _indicator;

    private InAppByAddViewProvider _inAppByAddViewProvider;
    private RewardedAdProvider _rewardedAdProvider;
    private GameProgressStorage _progressStorage;


    protected void OnEnable()
    {
        _inAppByAddViewProvider.ProgressChanged += UpdateIndicator;
        _button.onClick.AddListener(TryBuy);
    }

    protected void OnDisable()
    {
        _inAppByAddViewProvider.ProgressChanged -= UpdateIndicator;
        _button.onClick.RemoveListener(TryBuy);
    }

    public void Initialize(InAppByAddViewProvider inAppByAddViewProvider,
                        RewardedAdProvider rewardedAdProvider, GameProgressStorage progressStorage)
    {
        _inAppByAddViewProvider = inAppByAddViewProvider;
        _rewardedAdProvider = rewardedAdProvider;
        _progressStorage = progressStorage;
        UpdateIndicator(_inAppType);

        enabled = true;
    }


    private void UpdateIndicator(InAppType inAppType)
    {
        if (inAppType != _inAppType)
        {
            return;
        }

        int upgradeStage = _inAppByAddViewProvider.EarnedInAppWithAddProgress(inAppType);
        _indicator.SetStage(upgradeStage);
    }

    private void TryBuy()
    {
        _rewardedAdProvider.ShowAdvReward(_inAppType.ToString(), AddEarnProgress);
    }

    private void AddEarnProgress()
    {
        _inAppByAddViewProvider.AddUpgradeStage(_inAppType);
    }
}
