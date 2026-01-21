using SlimeGround.Menu.Windows.InAppPurchase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.GameShop
{
	public class InAppOffer : MonoBehaviour
	{
	    [SerializeField] private InAppType _inAppType;
	    [SerializeField] private Button _buyButton;
	    [SerializeField] private TextMeshProUGUI _priceText;
	    [SerializeField] private InAppsProvider _inAppProvider;

	    protected void OnEnable()
	    {
	        _buyButton.onClick.AddListener(TryBuy);
	    }

	    protected void OnDisable()
	    {
	        _buyButton.onClick.RemoveListener(TryBuy);
	    }

	    public void Initialize(InAppsProvider inAppPurchaseProvider)
	    {
	        _inAppProvider = inAppPurchaseProvider;
	    }

	    public void SetPrice(int price)
	    {
	        _priceText.text = price.ToString();
	    }

	    private void TryBuy()
	    {
	        _inAppProvider.BuyPurchase(_inAppType);
	    }
	}
}
