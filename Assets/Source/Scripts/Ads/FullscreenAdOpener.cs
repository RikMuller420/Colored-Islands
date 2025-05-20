using UnityEngine;
using YG;

public class FullscreenAdOpener
{
    private int _adMinLevelId = 3;
    private int _ñhangesBeforeAd = 1;
    private int _changesWithoutAd = 0;

    private LevelLoader _levelLoader;
    private RemoveAdsProvider _removeAdsProvider;

    public FullscreenAdOpener(LevelLoader levelLoader, RemoveAdsProvider removeAdsProvider)
    {
        _levelLoader = levelLoader;
        _removeAdsProvider = removeAdsProvider;

        _levelLoader.LevelChanged += OnLevelChanged;
        YandexGame.OpenFullAdEvent += OnAdOpened;
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
            YandexGame.FullscreenShow();
        }
    }

    private void OnAdOpened()
    {
        _changesWithoutAd = 0;
    }
}
