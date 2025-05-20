using System.Collections.Generic;
using System.Linq;
using YG;

public class InAppPurchaseProvider
{
    private IEnumerable<InAppSettingsData> _inAppPurchases;
    private WalletProvider _walletProvider;
    private BoostAmountProvider _boostProvider;
    private RemoveAdsProvider _removeAdsProvider;
    private InAppConfirmedWindow _inAppConfirmedWindow;

    public InAppPurchaseProvider(IEnumerable<InAppSettingsData> inAppPurchases, WalletProvider walletProvider,
                                             BoostAmountProvider boostProvider, RemoveAdsProvider removeAdsProvider,
                                             InAppConfirmedWindow inAppConfirmedWindow)
    {
        _inAppPurchases = inAppPurchases;
        _walletProvider = walletProvider;
        _boostProvider = boostProvider;
        _removeAdsProvider = removeAdsProvider;
        _inAppConfirmedWindow = inAppConfirmedWindow;
        YandexGame.PurchaseSuccessEvent += OnPurchaseSuccess;
    }

    public void BuyPurchase(InAppType inAppType)
    {
        InAppSettingsData inApp = _inAppPurchases.FirstOrDefault(inApp => inApp.Type == inAppType);
        YandexGame.BuyPayments(inApp.Id);
    }

    private void OnPurchaseSuccess(string id)
    {
        InAppSettingsData inApp = _inAppPurchases.FirstOrDefault(inApp => inApp.Id == id);
        ReceiveInApp(inApp);

        _inAppConfirmedWindow.Open(inApp);
    }

    private void ReceiveInApp(InAppSettingsData inApp)
    {
        InAppBonus lastBonus = inApp.InAppBonuses.Last();

        foreach (InAppBonus bonus in inApp.InAppBonuses)
        {
            bool isSaveAfterBonus = bonus == lastBonus;
            AddBonus(bonus, isSaveAfterBonus);
        }
    }

    private void AddBonus(InAppBonus bonus, bool isSaveAfterBonus)
    {
        switch (bonus.Type)
        {
            case InAppBonusType.Gold:
                _walletProvider.AddGold(bonus.Amount, isSaveAfterBonus);
                break;

            case InAppBonusType.BoostBundle:
                _boostProvider.AddBoostBundle(bonus.Amount, isSaveAfterBonus);
                break;

            case InAppBonusType.RemoveAdds:
                _removeAdsProvider.RemoveAds(isSaveAfterBonus);
                break;
        }
    }
}
