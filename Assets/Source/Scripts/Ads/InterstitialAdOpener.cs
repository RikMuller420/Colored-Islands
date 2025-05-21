public class InterstitialAdOpener
{
    private int _adMinLevelId = 3;
    private int _ñhangesBeforeAd = 1;
    private int _changesWithoutAd = 0;

    private LevelLoader _levelLoader;
    private RemoveAdsProvider _removeAdsProvider;
    private InterstitialAdProvider _interAdProvider;

    public InterstitialAdOpener(LevelLoader levelLoader, RemoveAdsProvider removeAdsProvider,
                              InterstitialAdProvider interAdProvider)
    {
        _levelLoader = levelLoader;
        _removeAdsProvider = removeAdsProvider;
        _interAdProvider = interAdProvider;

        _levelLoader.LevelChanged += OnLevelChanged;
        _interAdProvider.AdShowed += OnAdOpened;
    }

    private void OnLevelChanged()
    {
        if (_levelLoader.CurrentLevelData.Id < _adMinLevelId)
        {
            return;
        }

        _changesWithoutAd++;

        if (_removeAdsProvider.IsAdsRemoved)
        {
            return;
        }

        if (_changesWithoutAd > _ñhangesBeforeAd)
        {
            _interAdProvider.ShowAd();
        }
    }

    private void OnAdOpened()
    {
        _changesWithoutAd = 0;
    }
}
