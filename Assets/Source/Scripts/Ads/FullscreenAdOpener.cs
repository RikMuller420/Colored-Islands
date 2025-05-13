using UnityEngine;
using YG;

public class FullscreenAdOpener
{
    private int _adMinLevelId = 3;
    private int _ñhangesBeforeAd = 1;
    private int _changesWithoutAd = 0;

    private LevelLoader _levelLoader;
    private YandexGame _yandexGame;

    public FullscreenAdOpener(LevelLoader levelLoader, YandexGame yandexGame)
    {
        _levelLoader = levelLoader;
        _yandexGame = yandexGame;

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

        if (_changesWithoutAd > _ñhangesBeforeAd)
        {
            _yandexGame._FullscreenShow();
        }
    }

    private void OnAdOpened()
    {
        _changesWithoutAd = 0;
    }
}
