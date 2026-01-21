using SlimeGround.Integration.Ads;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Ads;
using UnityEngine;

namespace SlimeGround.Menu.Windows.GameShop
{
	public class GoldRewardByWatñhAddVideo : MonoBehaviour
	{
	    private const string RewardVideoId = "goldByWathAdd";
	    private const int GoldAmount = 100;

	    [SerializeField] private AddButton _buyButton;

	    private RewardedAdProvider _rewardedAdProvider;
	    private WalletProvider _walletProvider;

	    protected void OnEnable()
	    {
	        _buyButton.AviableClicked += TryBuy;
	    }

	    protected void OnDisable()
	    {
	        _buyButton.AviableClicked -= TryBuy;
	    }

	    public void Initialize(RewardedAdProvider rewardedAdProvider, WalletProvider walletProvider,
	                           FreeStuffCollDownProvider collDownProvider)
	    {
	        _rewardedAdProvider = rewardedAdProvider;
	        _walletProvider = walletProvider;
	        _buyButton.Initialize(collDownProvider);
	    }

	    private void TryBuy()
	    {
	        _rewardedAdProvider.ShowAdvReward(RewardVideoId, AddGold);
	        MetricSaver.ShowGetFreeGoldAdd();
	    }

	    private void AddGold()
	    {
	        _walletProvider.AddGold(GoldAmount);
	    }
	}
}
