using System;
using YG;

namespace SlimeGround.Integration.InAppPurchase
{

	public class InAppPurchaseProvider
	{
	    public event Action<string> SuccessPurchased;

	    public InAppPurchaseProvider()
	    {
	        YG2.onPurchaseSuccess += OnSuccessPurchased;
	    }

	    public void BuyInApp(string id)
	    {
	        YG2.BuyPayments(id);
	    }

	    private void OnSuccessPurchased(string id)
	    {
	        SuccessPurchased?.Invoke(id);
	    }

	}

}
