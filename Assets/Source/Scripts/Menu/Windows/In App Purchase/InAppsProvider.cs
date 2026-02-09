using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.InApps;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Integration.InAppPurchase;
using SlimeGround.Menu.Ads;

namespace SlimeGround.Menu.Windows.InAppPurchase
{
	public class InAppsProvider
	{
	    private IEnumerable<InAppSettingsData> _inAppPurchases;
	    private WalletProvider _walletProvider;
	    private BoostAmountProvider _boostProvider;
	    private RemoveAdsProvider _removeAdsProvider;
	    private InAppConfirmedWindow _inAppConfirmedWindow;
	    private InAppPurchaseProvider _inAppPurchaseProvider;
	    private InAppByAddViewProvider _inAppByAddViewProvider;
	    private PlayerDataProvider _playerData;

	    public InAppsProvider(IEnumerable<InAppSettingsData> inAppPurchases, WalletProvider walletProvider,
	                          BoostAmountProvider boostProvider, RemoveAdsProvider removeAdsProvider,
	                          InAppConfirmedWindow inAppConfirmedWindow, InAppPurchaseProvider inAppPurchaseProvider,
	                          InAppByAddViewProvider inAppByAddViewProvider, PlayerDataProvider playerData)
	    {
	        _inAppPurchases = inAppPurchases;
	        _walletProvider = walletProvider;
	        _boostProvider = boostProvider;
	        _removeAdsProvider = removeAdsProvider;
	        _inAppConfirmedWindow = inAppConfirmedWindow;
	        _inAppPurchaseProvider = inAppPurchaseProvider;
	        _inAppByAddViewProvider = inAppByAddViewProvider;
	        _playerData = playerData;

	        _inAppPurchaseProvider.SuccessPurchased += OnPurchaseSuccess;
	        _inAppByAddViewProvider.InAppProgressFinished += OnPurchaseSuccess;
	    }

		public void Dispose()
		{
			_inAppPurchaseProvider.SuccessPurchased -= OnPurchaseSuccess;
			_inAppByAddViewProvider.InAppProgressFinished -= OnPurchaseSuccess;
		}

	    public void BuyPurchase(InAppType inAppType)
	    {
	        InAppSettingsData inApp = _inAppPurchases.FirstOrDefault(inApp => inApp.Type == inAppType);
	        _inAppPurchaseProvider.BuyInApp(inApp.Id);
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
	            AddBonus(bonus);
	        }

	        _playerData.Save();
	    }

	    private void AddBonus(InAppBonus bonus)
	    {
	        switch (bonus.Type)
	        {
	            case InAppBonusType.Gold:
	                _walletProvider.AddGold(bonus.Amount);
	                break;

	            case InAppBonusType.BoostBundle:
	                _boostProvider.AddBoostBundle(bonus.Amount);
	                break;

	            case InAppBonusType.RemoveAdds:
	                _removeAdsProvider.RemoveAds();
	                break;
	        }

	        _playerData.Save();
	    }
	}
}
