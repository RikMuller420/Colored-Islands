using System;
using YG;

public class InterstitialAdProvider
{
    public event Action AdShowed;

    public InterstitialAdProvider()
    {
        YG2.onCloseInterAdvWasShow += OnAdShowed;
    }

    public void ShowAd()
    {
        YG2.InterstitialAdvShow();
    }

    private void OnAdShowed(bool isShowed)
    {
        if (isShowed)
        {
            AdShowed?.Invoke();
        }
    }
}
