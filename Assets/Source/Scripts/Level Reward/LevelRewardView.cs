using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardView : MonoBehaviour
{
    [SerializeField] private GameObject _goldIcon;
    [SerializeField] private TextMeshProUGUI _goldAmountText;
    [SerializeField] private GameObject _hatIcon;
    [SerializeField] private Image _hatImage;
    [SerializeField] private GameObject _rouletteIcon;
    [SerializeField] private TextMeshProUGUI _rouletteAmountText;
    [SerializeField] private List<BoostIconData> _boostIcons;

    private UnitsHatSettings _unitsHatSettings;
    private UpgradesProvider _upgradesProvider;

    public void Initialize(UnitsHatSettings unitsHatSettings, UpgradesProvider upgradesProvider)
    {
        _unitsHatSettings = unitsHatSettings;
        _upgradesProvider = upgradesProvider;
    }

    public void SetIcons(LevelRewardData levelRewardData, int multiplier = 1)
    {
        DeactivateIcons();

        if (IsHatInLevelReward(levelRewardData, out UnitHatData hat))
        {
            ActivateHatIcon(hat.Id);
        }

        if (levelRewardData.GoldAmount > 0)
        {
            int goldAmount = _upgradesProvider.CalculateUpgradedGoldAmount(levelRewardData.GoldAmount) * multiplier;
            ActivateGoldIcon(goldAmount);
        }

        if (levelRewardData.BoostAmount > 0)
        {
            ActivateBoostIcon(levelRewardData.BoostType, levelRewardData.BoostAmount * multiplier);
        }

        if (levelRewardData.RouletteSpinAmount > 0)
        {
            ActivateRouletteIcon(levelRewardData.RouletteSpinAmount * multiplier);
        }
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
        foreach (BoostIconData boostIcon in _boostIcons)
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
