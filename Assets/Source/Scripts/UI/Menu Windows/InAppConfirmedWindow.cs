using Lean.Localization;
using TMPro;
using UnityEngine;

public class InAppConfirmedWindow : MenuWindow
{
    private const string ReceivedLocalizationKey = "Already Received";

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Transform _iconHolder;

    private GameObject _currentIcon;

    public void Open(InAppSettingsData inAppSettings)
    {
        if (IsOpened)
        {
            return;
        }

        _title.text = LeanLocalization.GetTranslationText(inAppSettings.LocalizationKey) + " " +
                      LeanLocalization.GetTranslationText(ReceivedLocalizationKey);

        if (_currentIcon != null)
        {
            Destroy(_currentIcon);
        }

        _currentIcon = Instantiate(inAppSettings.IconPrefab, _iconHolder);

        base.Open();
    }
}
