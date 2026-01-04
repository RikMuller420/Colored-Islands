using System;

public class RemoveAdsProvider
{
    private GameProgressStorage _gameProgressStorage;

    public event Action RemoveAdsStateChanged;

    public RemoveAdsProvider(GameProgressStorage gameProgressStorage)
    {
        _gameProgressStorage = gameProgressStorage;
        gameProgressStorage.RemoveAdsStateChanged += OnRemoveAdsStateChanged;
    }

    public bool IsAdsRemoved => _gameProgressStorage.IsAdsRemoved;

    public void RemoveAds()
    {
        _gameProgressStorage.ApplyRemoveAddBonus();
    }

    private void OnRemoveAdsStateChanged()
    {
        RemoveAdsStateChanged?.Invoke();
    }
}
