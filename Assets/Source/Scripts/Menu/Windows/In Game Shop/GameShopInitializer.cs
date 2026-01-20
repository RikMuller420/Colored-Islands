using UnityEngine;

public class GameShopInitializer : MonoBehaviour
{
    [SerializeField] private InAppSettings _inAppSettings;
    [SerializeField] private PlayerDataProvider _playerData;
    [SerializeField] private FreeStuffCollDownProvider _collDownProvider;
    [SerializeField] private InAppPurchaseInitializer _inAppPurchaseInitializer;

    [SerializeField] private WalletView _walletView;
    [SerializeField] private InGameShopInitializer _inGameShopInitializer;

    public void Initialize(UpgradesProvider upgradesProvider, RewardedAdProvider rewardedAdProvider,
                            BoostAmountProvider boostAmountProvider, RemoveAdsProvider removeAdsProvider, WalletProvider walletProvider)
    {
        var inAppByAddViewProvider = new InAppByAddViewProvider(_playerData, _inAppSettings);
        var stickyAdProvider = new StickyAdProvider();
        var inAppPurchaseProvider = new InAppPurchaseProvider();

        _walletView.Initialize(walletProvider);

        _inGameShopInitializer.Initialize(upgradesProvider, boostAmountProvider, walletProvider);
        _inAppPurchaseInitializer.Initialize(walletProvider, boostAmountProvider, removeAdsProvider, inAppPurchaseProvider,
                                             inAppByAddViewProvider, rewardedAdProvider, stickyAdProvider);
    }
}
