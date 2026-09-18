using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.GameShop
{
	public abstract class OfferLine : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _priceText;
		[SerializeField] private Button _buyButton;


		private Color _ableToBuyColor = new Color(0.23f, 0.11f, 0.1f);
		private Color _notAbleToBuyColor = new Color(0.63f, 0.04f, 0.1f);

		protected Button BuyButton => _buyButton;
		protected WalletProvider WalletProvider { get; private set; }
		protected int GoldPrice { get; private set; }

		protected virtual void OnEnable()
		{
			WalletProvider.GoldAmountChanged += OnGoldAmountChanged;
			OnGoldAmountChanged(WalletProvider.GoldAmount);
		}

		protected virtual void OnDisable()
		{
			WalletProvider.GoldAmountChanged -= OnGoldAmountChanged;
		}

		public void Initialize(WalletProvider walletProvider, int goldPrice)
		{
			WalletProvider = walletProvider;
			GoldPrice = goldPrice;
			_priceText.text = GoldPrice.ToString();
			OnGoldAmountChanged(walletProvider.GoldAmount);
			enabled = true;
		}

		private void OnGoldAmountChanged(int aviableGold)
		{
			bool isAbleToBuy = aviableGold >= GoldPrice;

			_priceText.color = isAbleToBuy ? _ableToBuyColor : _notAbleToBuyColor;
			_buyButton.interactable = isAbleToBuy;
		}
	}
}
