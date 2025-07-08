using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardWindow : MenuWindow
{
    private const string RewardedAddId = "DoubleLevelReward";

    [SerializeField] private GameObject _goldIcon;
    [SerializeField] private TextMeshProUGUI _goldAmountText;
    [SerializeField] private GameObject _hatIcon;
    [SerializeField] private Image _hatImage;
    [SerializeField] private GameObject _rouletteIcon;
    [SerializeField] private TextMeshProUGUI _rouletteAmountText;
    [SerializeField] private List<BoostIconData> _boostIcons;
    [SerializeField] private Button _receiveButton;
    [SerializeField] private Button _receiveWithAdsButton;

    private UnitsHatSettings _unitsHatSettings;
    private GameProgressStorage _progressStorage;
    private RewardedAdProvider _rewardedAdProvider;

    private LevelRewardData _currentReward;
    private int _adsMultiplier = 2;

    protected override void OnEnable()
    {
        _receiveButton.onClick.AddListener(ReceiveReward);
        _receiveWithAdsButton.onClick.AddListener(TryReceiveRewardWithAdd);
    }

    protected override void OnDisable()
    {
        _receiveButton.onClick.RemoveListener(ReceiveReward);
        _receiveWithAdsButton.onClick.RemoveListener(TryReceiveRewardWithAdd);
    }

    public void Initialize(UnitsHatSettings unitsHatSettings, GameProgressStorage progressStorage,
                            RewardedAdProvider rewardedAdProvider)
    {
        _unitsHatSettings = unitsHatSettings;
        _progressStorage = progressStorage;
        _rewardedAdProvider = rewardedAdProvider;
    }

    public void Open(LevelRewardData levelRewardData)
    {
        _currentReward = levelRewardData;
        DeactivateIcons();

        if (IsHatInLevelReward(levelRewardData, out UnitHatData hat))
        {
            ActivateHatIcon(hat.Id);
        }

        if (levelRewardData.GoldAmount > 0)
        {
            ActivateGoldIcon(levelRewardData.GoldAmount);
        }

        if (levelRewardData.BoostAmount > 0)
        {
            ActivateBoostIcon(levelRewardData.BoostType, levelRewardData.BoostAmount);
        }

        if (levelRewardData.RouletteSpinAmount > 0)
        {
            ActivateRouletteIcon(levelRewardData.RouletteSpinAmount);
        }

        base.OpenUnclosableWindow();
    }

    private void ReceiveReward()
    {
        AddReward();
        Close();
    }

    private void TryReceiveRewardWithAdd()
    {
        _rewardedAdProvider.ShowAdvReward(RewardedAddId, ReceiveRewardWithAdd);
    }

    private void ReceiveRewardWithAdd()
    {
        AddReward(_adsMultiplier);
        Close();
    }

    private void AddReward(int multiplier = 1)
    {
        if (_currentReward.GoldAmount > 0)
        {
            int newGoldAmount = _progressStorage.GoldAmount + _currentReward.GoldAmount * multiplier;
            _progressStorage.SetGoldAmount(newGoldAmount);
        }

        if (_currentReward.RouletteSpinAmount > 0)
        {
            int newSpinAmount = _progressStorage.AviableSpinCount + _currentReward.RouletteSpinAmount * multiplier;
            _progressStorage.SetSpinCount(newSpinAmount);
        }

        if (_currentReward.BoostAmount > 0)
        {
            int boostAmount = _progressStorage.GetBoostAmount(_currentReward.BoostType) + _currentReward.BoostAmount * multiplier;
            _progressStorage.SetBoostAmount(_currentReward.BoostType, boostAmount);
        }

        _progressStorage.MarkLevelRewardReceived(_currentReward.LevelId);
        _progressStorage.Save();
    }

    private void ActivateRouletteIcon(int amount)
    {
        _rouletteIcon.SetActive(true);
        _rouletteAmountText.text = amount.ToString();
    }

    private void ActivateHatIcon(int hatId)
    {
        _hatIcon.SetActive(true);
        Sprite hat = _unitsHatSettings.Hats.FirstOrDefault(hat => hat.Id == hatId).SelectSprite;
        _hatImage.sprite = hat;
    }

    private void ActivateBoostIcon(BoostType boostType, int amount)
    {
        BoostIconData boostIcon = _boostIcons.FirstOrDefault(boostIcon => boostIcon.Type == boostType);
        boostIcon.Icon.SetActive(true);
        boostIcon.AmountText.text = amount.ToString();
    }

    private void ActivateGoldIcon(int goldAmount)
    {
        _goldIcon.SetActive(true);
        _goldAmountText.text = goldAmount.ToString();
    }

    private void DeactivateIcons()
    {
        foreach(BoostIconData boostIcon in _boostIcons)
        {
            boostIcon.Icon.SetActive(false);
        }

        _hatIcon.SetActive(false);
        _goldIcon.SetActive(false);
        _rouletteIcon.SetActive(false);
    }

    private bool IsHatInLevelReward(LevelRewardData levelRewardData, out UnitHatData hat)
    {
        hat = _unitsHatSettings.Hats.FirstOrDefault(hat => hat.RequredLevel == levelRewardData.LevelId);

        return hat != null;
    }
}
