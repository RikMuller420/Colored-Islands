using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Windows.Roulette;
using TMPro;
using UnityEngine;

namespace SlimeGround.Menu.Windows.GameShop
{
	public class RouletteSpinOfferLine : OfferLine
	{
		[SerializeField] private TextMeshProUGUI _amountText;

		private RouletteSpinProvider _spinAmountProvider;

		protected override void OnEnable()
		{
			base.OnEnable();
			_spinAmountProvider.SpinAmountChanged += OnSpinAmountChanged;
			BuyButton.onClick.AddListener(BuyRouletteSpin);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_spinAmountProvider.SpinAmountChanged -= OnSpinAmountChanged;
			BuyButton.onClick.RemoveListener(BuyRouletteSpin);
		}

		public void Initialize(RouletteSpinProvider spinAmountProvider, WalletProvider walletProvider)
		{
			int goldPrice = spinAmountProvider.RouletteSettings.SpinGoldPrice;
			Initialize(walletProvider, goldPrice);

			_spinAmountProvider = spinAmountProvider;
			OnSpinAmountChanged();
			enabled = true;
		}

		private void OnSpinAmountChanged()
		{
			_amountText.text = _spinAmountProvider.SpinAmount.ToString();
		}

		private void BuyRouletteSpin()
		{
			WalletProvider.SpendGold(GoldPrice);
			_spinAmountProvider.AddSpin();
			MetricSaver.BuyRouletteSpin();
		}
	}
}
