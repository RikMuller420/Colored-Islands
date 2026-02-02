using SlimeGround.Integration.Ads;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Ads;
using SlimeGround.Menu.Windows.InAppPurchase;
using UnityEngine;

namespace SlimeGround.Menu.Windows.GameShop
{
	public class InAppByAddViewOffer : MonoBehaviour
	{
	    [SerializeField] private InAppType _inAppType;
	    [SerializeField] private AddButton _button;
	    [SerializeField] private UpgradeIndicator _indicator;

	    private InAppByAddViewProvider _inAppByAddViewProvider;
	    private RewardedAdProvider _rewardedAdProvider;

	    protected void OnEnable()
	    {
	        _inAppByAddViewProvider.ProgressChanged += UpdateIndicator;
	        _button.AviableClicked += TryBuy;
	    }

	    protected void OnDisable()
	    {
	        _inAppByAddViewProvider.ProgressChanged -= UpdateIndicator;
	        _button.AviableClicked -= TryBuy;
	    }

	    public void Initialize(InAppByAddViewProvider inAppByAddViewProvider,
							   RewardedAdProvider rewardedAdProvider,
							   FreeStuffCollDownProvider collDownProvider)
	    {
	        _inAppByAddViewProvider = inAppByAddViewProvider;
	        _rewardedAdProvider = rewardedAdProvider;
	        _button.Initialize(collDownProvider);
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
	        MetricSaver.GetInAppViaWathAdd(_inAppType);
	    }

	    private void AddEarnProgress()
	    {
	        _inAppByAddViewProvider.AddUpgradeStage(_inAppType);
	    }
	}
}
