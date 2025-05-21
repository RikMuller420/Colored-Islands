using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostBuyConfirmationWindow : MenuWindow
{
    private const string RewardVideoId = "boost";

    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyWithGoldButton;
    [SerializeField] private Button _buyWithAddButton;

    [SerializeField] private List<BoostIconData> _boostIcons;

    private BoostAmountProvider _boostAmountProvider;
    private WalletProvider _walletProvider;
    private BoostSettings _boostSettings;
    private RewardedAdProvider _rewardedAdProvider;

    private BoostType _currentBoostType;
    private int _currentBoostPrice;

    private Color _ableToBuyColor = new Color(0.23f, 0.11f, 0.1f);
    private Color _notAbleToBuyColor = new Color(0.63f, 0.04f, 0.1f);

    private new void OnEnable()
    {
        base.OnEnable();
        _buyWithGoldButton.onClick.AddListener(BuyBoostWithGold);
        _buyWithAddButton.onClick.AddListener(BuyBoostWithAdd);
    }

    private new void OnDisable()
    {
        base.OnDisable();
        _buyWithGoldButton.onClick.RemoveListener(BuyBoostWithGold);
        _buyWithAddButton.onClick.RemoveListener(BuyBoostWithAdd);
    }

    public void Initialize(BoostAmountProvider boostAmountProvider, BoostSettings boostSettings,
                           WalletProvider walletProvider, RewardedAdProvider rewardedAdProvider)
    {
        _boostAmountProvider = boostAmountProvider;
        _boostSettings = boostSettings;
        _walletProvider = walletProvider;
        _rewardedAdProvider = rewardedAdProvider;
    }

    public void Open(BoostType boostType)
    {
        if (IsOpened)
        {
            return;
        }

        _currentBoostType = boostType;
        _currentBoostPrice = _boostSettings.Boosts.FirstOrDefault(boost => boost.Type == _currentBoostType).GoldPrice;
        SetWindowTexts();

        Time.timeScale = 0f;
        base.Open();
    }

    public override void Close()
    {
        if (IsOpened == false)
        {
            return;
        }

        Time.timeScale = 1f;
        base.Close();
    }

    private void BuyBoostWithGold()
    {
        _walletProvider.SpendGold(_currentBoostPrice);
        _boostAmountProvider.AddBoost(_currentBoostType);
        Close();
    }

    private void BuyBoostWithAdd()
    {
        _rewardedAdProvider.ShowAdvReward(RewardVideoId, AddBoost);
        Close();
    }

    private void AddBoost()
    {
        _boostAmountProvider.AddBoost(_currentBoostType);
    }

    private void SetWindowTexts()
    {
        foreach (BoostIconData boostIcon in _boostIcons)
        {
            bool isIconActive = boostIcon.Type == _currentBoostType;
            boostIcon.Icon.gameObject.SetActive(isIconActive);
        }

        _priceText.text = _currentBoostPrice.ToString();

        bool isAbleToBuy = _walletProvider.GoldAmount >= _currentBoostPrice;
        _priceText.color = isAbleToBuy ? _ableToBuyColor : _notAbleToBuyColor;

        _buyWithGoldButton.gameObject.SetActive(isAbleToBuy);
        _buyWithAddButton.gameObject.SetActive(!isAbleToBuy);
    }
}
