using System.Linq;

public class InAppPurchase
{
    private readonly InAppSettingsData _inAppSettings;
    private readonly WalletProvider _walletProvider;
    private readonly BoostAmountProvider _boostProvider;
    private readonly RemoveAdsProvider _removeAdsProvider;

    public InAppPurchase(InAppSettingsData inAppSettings, WalletProvider walletProvider,
                         BoostAmountProvider boostProvider, RemoveAdsProvider removeAdsProvider)
    {
        _inAppSettings = inAppSettings;
        _walletProvider = walletProvider;
        _boostProvider = boostProvider;
        _removeAdsProvider = removeAdsProvider;
    }

    public InAppSettingsData Settings => _inAppSettings;

    public void Receive()
    {
        InAppBonus lastBonus = Settings.InAppBonuses.Last();

        foreach (InAppBonus bonus in Settings.InAppBonuses)
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
