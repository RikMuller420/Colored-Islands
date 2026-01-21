using System.Linq;
using SlimeGround.Data.ScriptableObjects.Upgrades;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.GameShop
{
	public class UpgradeOfferLine : MonoBehaviour
	{
	    [SerializeField] private UpgradeType _upgradeType;
	    [SerializeField] private TextMeshProUGUI _priceText;
	    [SerializeField] private UpgradeIndicator _upgradeIndicator;
	    [SerializeField] private Button _buyButton;
	    [SerializeField] private GameObject _priceHolder;
	    [SerializeField] private GameObject _maxAmountHint;

	    private UpgradesProvider _upgradesProvider;
	    private WalletProvider _walletProvider;
	    private UpgradeSettingsData _upgradeSettings;
	    private int _goldPrice;

	    private Color _ableToBuyColor = new Color(0.23f, 0.11f, 0.1f);
	    private Color _notAbleToBuyColor = new Color(0.63f, 0.04f, 0.1f);

	    private void OnEnable()
	    {
	        _upgradesProvider.Upgraded += OnUpgraded;
	        _walletProvider.GoldAmountChanged += UpdateBuyAviability;
	        _buyButton.onClick.AddListener(BuyUpgrade);

	        UpdateBuyAviability(_walletProvider.GoldAmount);
	    }

	    private void OnDisable()
	    {
	        _upgradesProvider.Upgraded -= OnUpgraded;
	        _walletProvider.GoldAmountChanged -= UpdateBuyAviability;
	        _buyButton.onClick.RemoveListener(BuyUpgrade);
	    }

	    public void Initialize(UpgradesProvider upgradesProvider, UpgradeSettings upgradeSettings,
	                            WalletProvider walletProvider)
	    {
	        _upgradesProvider = upgradesProvider;
	        _walletProvider = walletProvider;
	        _upgradeSettings = upgradeSettings.Upgrades.FirstOrDefault(upgrade => upgrade.Type == _upgradeType); ;
	        UpdatePrice();
	        OnUpgraded(_upgradeType);
	        UpdateBuyAviability(walletProvider.GoldAmount);
	        enabled = true;
	    }

	    private void OnUpgraded(UpgradeType upgradeType)
	    {
	        if (upgradeType != _upgradeType)
	        {
	            return;
	        }

	        int upgradeStage = _upgradesProvider.UpgradeStage(_upgradeType);
	        _upgradeIndicator.SetStage(upgradeStage);

	        if (upgradeStage == _upgradeSettings.GoldPrices.Count)
	        {
	            _buyButton.gameObject.SetActive(false);
	            _priceHolder.SetActive(false);
	            _maxAmountHint.SetActive(true);
	        }

	        UpdatePrice();
	        UpdateBuyAviability(_walletProvider.GoldAmount);
	    }

	    private void UpdateBuyAviability(int aviableGold)
	    {
	        bool isAbleToBuy = aviableGold >= _goldPrice;

	        _priceText.color = isAbleToBuy ? _ableToBuyColor : _notAbleToBuyColor;
	        _buyButton.interactable = isAbleToBuy;
	    }

	    private void BuyUpgrade()
	    {
	        _walletProvider.SpendGold(_goldPrice);
	        _upgradesProvider.AddUpgradeStage(_upgradeType);
	        MetricSaver.BuyUpgrade(_upgradeType);
	    }

	    private void UpdatePrice()
	    {
	        int stage = _upgradesProvider.UpgradeStage(_upgradeType);

	        if (stage >= _upgradeSettings.GoldPrices.Count)
	        {
	            return;
	        }

	        _goldPrice = _upgradeSettings.GoldPrices[stage];
	        _priceText.text = _goldPrice.ToString();
	    }
	}
}
