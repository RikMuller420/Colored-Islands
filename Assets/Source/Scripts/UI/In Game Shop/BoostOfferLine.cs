using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostOfferLine : MonoBehaviour
{
    [SerializeField] private BoostType _boostType;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _boostAmountText;
    [SerializeField] private Button _buyButton;

    private BoostAmountProvider _boostAmountProvider;
    private WalletProvider _walletProvider;
    private int _goldPrice;

    private Color _ableToBuyColor = new Color(0.23f, 0.11f, 0.1f);
    private Color _notAbleToBuyColor = new Color(0.63f, 0.04f, 0.1f);

    private void OnEnable()
    {
        _boostAmountProvider.BoostsAmountChanged += OnBoostAmountChanged;
        _walletProvider.GoldAmountChanged += OnGoldAmountChanged;
        _buyButton.onClick.AddListener(BuyBoost);

        OnGoldAmountChanged(_walletProvider.GoldAmount);
    }

    private void OnDisable()
    {
        _boostAmountProvider.BoostsAmountChanged -= OnBoostAmountChanged;
        _walletProvider.GoldAmountChanged -= OnGoldAmountChanged;
        _buyButton.onClick.RemoveListener(BuyBoost);
    }

    public void Initialize(BoostAmountProvider boostAmountProvider, BoostSettings boostSettings,
                            WalletProvider walletProvider)
    {
        _boostAmountProvider = boostAmountProvider;
        _walletProvider = walletProvider;
        _goldPrice = boostSettings.Boosts.FirstOrDefault(boost => boost.Type == _boostType).GoldPrice;
        _priceText.text = _goldPrice.ToString();
        OnBoostAmountChanged(_boostType);
        OnGoldAmountChanged(walletProvider.GoldAmount);
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

    private void OnGoldAmountChanged(int aviableGold)
    {
        bool isAbleToBuy = aviableGold >= _goldPrice;

        _priceText.color = isAbleToBuy ? _ableToBuyColor : _notAbleToBuyColor;
        _buyButton.interactable = isAbleToBuy;
    }

    private void BuyBoost()
    {
        _walletProvider.SpendGold(_goldPrice);
        _boostAmountProvider.AddBoost(_boostType);
        MetricSaver.BuyBoost(_boostType);
    }
}
