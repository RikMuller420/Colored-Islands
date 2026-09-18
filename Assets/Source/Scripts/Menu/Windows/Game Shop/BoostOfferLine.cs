using System.Linq;
using SlimeGround.Data.ScriptableObjects.Boosts;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Integration.Metrics;
using TMPro;
using UnityEngine;

namespace SlimeGround.Menu.Windows.GameShop
{
	public class BoostOfferLine : OfferLine
	{
	    [SerializeField] private BoostType _boostType;
	    [SerializeField] private TextMeshProUGUI _boostAmountText;

	    private BoostAmountProvider _boostAmountProvider;

		protected override void OnEnable()
	    {
			base.OnEnable();
	        _boostAmountProvider.BoostsAmountChanged += OnBoostAmountChanged;
	        BuyButton.onClick.AddListener(BuyBoost);
	    }

		protected override void OnDisable()
	    {
			base.OnDisable();
			_boostAmountProvider.BoostsAmountChanged -= OnBoostAmountChanged;
			BuyButton.onClick.RemoveListener(BuyBoost);
	    }

	    public void Initialize(BoostAmountProvider boostAmountProvider, BoostSettings boostSettings,
	                            WalletProvider walletProvider)
	    {
	        int goldPrice = boostSettings.Boosts.FirstOrDefault(boost => boost.Type == _boostType).GoldPrice;
			Initialize(walletProvider, goldPrice);

	        _boostAmountProvider = boostAmountProvider;
	        OnBoostAmountChanged(_boostType);
	        enabled = true;
	    }

	    private void OnBoostAmountChanged(BoostType boostType)
	    {
	        if (boostType != _boostType)
	        {
	            return;
	        }

	        int boostAmount = _boostAmountProvider.BoostAmount(_boostType);
	        _boostAmountText.text = boostAmount.ToString();
	    }

	    private void BuyBoost()
	    {
	        WalletProvider.SpendGold(GoldPrice);
	        _boostAmountProvider.AddBoost(_boostType);
	        MetricSaver.TrackBoostPurchased(_boostType);
	    }
	}
}
