using UnityEngine;

public class RemoveAdsAviabilityUpdater : MonoBehaviour
{
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _maxAmountHint;
    [SerializeField] private GameObject _earnWithAddZone;

    private RemoveAdsProvider _removeAdsProvider;
    private StickyAdProvider _stickyAdProvider;

    private void OnEnable()
    {
        _removeAdsProvider.RemoveAdsStateChanged += RemoveAdsStateChanged;
    }

    private void OnDisable()
    {
        _removeAdsProvider.RemoveAdsStateChanged += RemoveAdsStateChanged;
    }

    public void Initialize(RemoveAdsProvider removeAdsProvider, StickyAdProvider stickyAdProvide)
    {
        _removeAdsProvider = removeAdsProvider;
        _stickyAdProvider = stickyAdProvide;
        RemoveAdsStateChanged();
        enabled = true;
    }

    private void RemoveAdsStateChanged()
    {
        bool isAdsRemoved = _removeAdsProvider.IsAdsRemoved;

        _buyButton.SetActive(isAdsRemoved == false);
        _earnWithAddZone.SetActive(isAdsRemoved == false);
        _maxAmountHint.SetActive(isAdsRemoved);

        if (isAdsRemoved)
        {
            _stickyAdProvider.DeactivateStickyAds();
        }
    }
}
