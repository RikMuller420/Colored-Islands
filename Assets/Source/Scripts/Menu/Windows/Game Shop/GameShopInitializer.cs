using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.InApps;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Integration.Ads;
using SlimeGround.Integration.InAppPurchase;
using SlimeGround.Menu.Ads;
using SlimeGround.Menu.Wallet;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using SlimeGround.Menu.Windows.InAppPurchase;
using UnityEngine;

namespace SlimeGround.Menu.Windows.GameShop
{
	public class GameShopInitializer : MonoBehaviour
	{
	    [SerializeField] private InAppSettings _inAppSettings;
	    [SerializeField] private PlayerDataProvider _playerData;
	    [SerializeField] private FreeStuffCollDownProvider _collDownProvider;
	    [SerializeField] private InAppPurchaseInitializer _inAppPurchaseInitializer;

	    [SerializeField] private WalletView _walletView;
	    [SerializeField] private InGameShopInitializer _inGameShopInitializer;

		private InAppPurchaseProvider _inAppPurchaseProvider;

		public void Initialize(UpgradesProvider upgradesProvider, RewardedAdProvider rewardedAdProvider,
	                           BoostAmountProvider boostAmountProvider, RemoveAdsProvider removeAdsProvider, WalletProvider walletProvider)
	    {
	        var inAppByAddViewProvider = new InAppByAddViewProvider(_playerData, _inAppSettings);
	        var stickyAdProvider = new StickyAdProvider();
			_inAppPurchaseProvider = new InAppPurchaseProvider();

	        _walletView.Initialize(walletProvider);

	        _inGameShopInitializer.Initialize(upgradesProvider, boostAmountProvider, walletProvider);
	        _inAppPurchaseInitializer.Initialize(walletProvider, boostAmountProvider, removeAdsProvider, _inAppPurchaseProvider,
	                                             inAppByAddViewProvider, rewardedAdProvider, stickyAdProvider);
	    }

		public void Dispose()
		{
			_inAppPurchaseProvider.Dispose();
			_inAppPurchaseInitializer.Dispose();
		}
	}
}
