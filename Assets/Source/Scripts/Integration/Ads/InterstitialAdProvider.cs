using System;
using YG;

namespace SlimeGround.Integration.Ads
{
	public class InterstitialAdProvider
	{
	    public InterstitialAdProvider()
	    {
	        YG2.onCloseInterAdvWasShow += OnAdShowed;
	    }

		public event Action AdShowed;

		public void Dispose()
		{
			YG2.onCloseInterAdvWasShow -= OnAdShowed;
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
}
