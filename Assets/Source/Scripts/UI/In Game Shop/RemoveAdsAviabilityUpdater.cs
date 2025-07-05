using UnityEngine;

public class RemoveAdsAviabilityUpdater : MonoBehaviour
{
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _maxAmountHint;
    [SerializeField] private GameObject _earnWithAddZone;

    private RemoveAdsProvider _removeAdsProvider;

    private void OnEnable()
    {
        _removeAdsProvider.RemoveAdsStateChanged += RemoveAdsStateChanged;
    }

    private void OnDisable()
    {
        _removeAdsProvider.RemoveAdsStateChanged += RemoveAdsStateChanged;
    }

    public void Initialize(RemoveAdsProvider removeAdsProvider)
    {
        _removeAdsProvider = removeAdsProvider;
        RemoveAdsStateChanged();
        enabled = true;
    }

    private void RemoveAdsStateChanged()
    {
        _buyButton.SetActive(_removeAdsProvider.IsAdsRemoved == false);
        _earnWithAddZone.SetActive(_removeAdsProvider.IsAdsRemoved == false);
        _maxAmountHint.SetActive(_removeAdsProvider.IsAdsRemoved);
    }
}
