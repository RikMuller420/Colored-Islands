using System.Collections.Generic;
using UnityEngine;

public class InAppPurchaseInitializer : MonoBehaviour
{
    [SerializeField] private InAppSettings _inAppSettings;

    [SerializeField] private PlayerDataProvider _playerData;
    [SerializeField] private FreeStuffCollDownProvider _collDownProvider;
    [SerializeField] private InAppConfirmedWindow _inAppConfirmedWindow;
    [SerializeField] private RemoveAdsAviabilityUpdater _removeAdsAviabilityUpdater;
    [SerializeField] private List<InAppOffer> _inAppOffers = new();
    [SerializeField] private List<InAppByAddViewOffer> _inAppByAddViewOffers = new();
    [SerializeField] private GoldRewardByWathAddOffer _goldRewardByWathAddOffer;

    public void Initialize(WalletProvider walletProvider, BoostAmountProvider boostProvider,
                           RemoveAdsProvider removeAdsProvider, InAppPurchaseProvider inAppPurchaseProvider,
                           InAppByAddViewProvider inAppByAddViewProvider, RewardedAdProvider rewardedAdProvider,
                           StickyAdProvider stickyAdProvider)
    {
        var inAppProvider = new InAppsProvider(_inAppSettings.InApps, walletProvider, boostProvider,
                                               removeAdsProvider, _inAppConfirmedWindow, inAppPurchaseProvider,
                                               inAppByAddViewProvider, _playerData);

        _removeAdsAviabilityUpdater.Initialize(removeAdsProvider, stickyAdProvider);

        foreach (InAppOffer inAppOffer in _inAppOffers)
        {
            inAppOffer.Initialize(inAppProvider);
        }

        foreach (InAppByAddViewOffer inAppByAddViewOffer in _inAppByAddViewOffers)
        {
            inAppByAddViewOffer.Initialize(inAppByAddViewProvider, rewardedAdProvider, _collDownProvider);
        }

        _goldRewardByWathAddOffer.Initialize(rewardedAdProvider, walletProvider, _collDownProvider);
    }
}
