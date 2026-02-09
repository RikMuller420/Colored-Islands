using System.Collections.Generic;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.InApps;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Integration.Ads;
using SlimeGround.Integration.InAppPurchase;
using SlimeGround.Menu.Ads;
using SlimeGround.Menu.Windows.GameShop;
using UnityEngine;

namespace SlimeGround.Menu.Windows.InAppPurchase
{
	public class InAppPurchaseInitializer : MonoBehaviour
	{
	    [SerializeField] private InAppSettings _inAppSettings;

	    [SerializeField] private PlayerDataProvider _playerData;
	    [SerializeField] private FreeStuffCollDownProvider _collDownProvider;
	    [SerializeField] private InAppConfirmedWindow _inAppConfirmedWindow;
	    [SerializeField] private RemoveAdsAviabilityUpdater _removeAdsAviabilityUpdater;
	    [SerializeField] private List<InAppOffer> _inAppOffers = new();
	    [SerializeField] private List<InAppByAddViewOffer> _inAppByAddViewOffers = new();
	    [SerializeField] private GoldRewardByWatñhAddVideo _goldRewardByWathAddOffer;

		private InAppsProvider _inAppProvider;

		public void Initialize(WalletProvider walletProvider, BoostAmountProvider boostProvider,
	                           RemoveAdsProvider removeAdsProvider, InAppPurchaseProvider inAppPurchaseProvider,
	                           InAppByAddViewProvider inAppByAddViewProvider, RewardedAdProvider rewardedAdProvider,
	                           StickyAdProvider stickyAdProvider)
	    {
			_inAppProvider = new InAppsProvider(_inAppSettings.InApps, walletProvider, boostProvider,
	                                               removeAdsProvider, _inAppConfirmedWindow, inAppPurchaseProvider,
	                                               inAppByAddViewProvider, _playerData);

	        _removeAdsAviabilityUpdater.Initialize(removeAdsProvider, stickyAdProvider);

	        foreach (InAppOffer inAppOffer in _inAppOffers)
	        {
	            inAppOffer.Initialize(_inAppProvider);
	        }

	        foreach (InAppByAddViewOffer inAppByAddViewOffer in _inAppByAddViewOffers)
	        {
	            inAppByAddViewOffer.Initialize(inAppByAddViewProvider, rewardedAdProvider, _collDownProvider);
	        }

	        _goldRewardByWathAddOffer.Initialize(rewardedAdProvider, walletProvider, _collDownProvider);
	    }

		public void Dispose()
		{
			_inAppProvider.Dispose();

			foreach (InAppByAddViewOffer inAppByAddViewOffer in _inAppByAddViewOffers)
			{
				inAppByAddViewOffer.Dispose();
			}

			_goldRewardByWathAddOffer.Dispose();
		}
	}
}
