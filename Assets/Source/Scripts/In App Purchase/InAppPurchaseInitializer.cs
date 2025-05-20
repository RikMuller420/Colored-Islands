using System.Collections.Generic;
using UnityEngine;

public class InAppPurchaseInitializer : MonoBehaviour
{
    [SerializeField] private InAppSettings _inAppSettings;
    [SerializeField] private InAppConfirmedWindow _inAppConfirmedWindow;
    [SerializeField] private RemoveAdsAviabilityUpdater _removeAdsAviabilityUpdater;
    [SerializeField] private List<InAppOffer> _inAppOffers = new();

    public void Initialize(WalletProvider walletProvider, BoostAmountProvider boostProvider, RemoveAdsProvider removeAdsProvider)
    {
        var inAppPurchaseProvider = new InAppPurchaseProvider(_inAppSettings.InApps, walletProvider, boostProvider,
                                                             removeAdsProvider, _inAppConfirmedWindow);

        _removeAdsAviabilityUpdater.Initialize(removeAdsProvider);

        foreach (InAppOffer inAppOffer in _inAppOffers)
        {
            inAppOffer.Initialize(inAppPurchaseProvider);
        }
    }
}
