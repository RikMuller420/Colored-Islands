using System;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Integration.Ads;

namespace SlimeGround.Menu.Ads
{
	public class InterstitialAdOpener
	{
	    private int _adMinLevelId = 3;
	    private int _loadsBeforeAd = 0;
	    private float _adCooldownSeconds = 30f;

	    private int _currentLoadsWithoutAd = 0;
	    private DateTime _lastAdTime;

	    private LevelChangeEventTracker _levelChangeEventTracker;
	    private RemoveAdsProvider _removeAdsProvider;
	    private InterstitialAdProvider _interAdProvider;
	    private RewardedAdProvider _rewardedAdProvider;

	    public InterstitialAdOpener(LevelChangeEventTracker levelChangeEventTracker, RemoveAdsProvider removeAdsProvider,
	                              InterstitialAdProvider interAdProvider, RewardedAdProvider rewardedAdProvider)
	    {
	        _levelChangeEventTracker = levelChangeEventTracker;
	        _removeAdsProvider = removeAdsProvider;
	        _interAdProvider = interAdProvider;
	        _rewardedAdProvider = rewardedAdProvider;

	        _levelChangeEventTracker.LevelChanged += OnLevelChanged;
	        _interAdProvider.AdShowed += OnInterAdOpened;
	        _rewardedAdProvider.RewardedAdClosed += ResetAdTimer;
	    }

		public void Dispose()
		{
			_levelChangeEventTracker.LevelChanged -= OnLevelChanged;
			_interAdProvider.AdShowed -= OnInterAdOpened;
			_rewardedAdProvider.RewardedAdClosed -= ResetAdTimer;
		}

	    private void OnLevelChanged(ILevelData levelData)
	    {
	        _currentLoadsWithoutAd++;

	        if (levelData.LevelId < _adMinLevelId)
	        {
	            return;
	        }

	        if (_removeAdsProvider.IsAdsRemoved)
	        {
	            return;
	        }

	        float secondsFromLastAd = (float)(DateTime.Now - _lastAdTime).TotalSeconds;

	        if (secondsFromLastAd < _adCooldownSeconds)
	        {
	            return;
	        }

	        if (_currentLoadsWithoutAd > _loadsBeforeAd)
	        {
	            _interAdProvider.ShowAd();
	        }
	    }

	    private void OnInterAdOpened()
	    {
	        _currentLoadsWithoutAd = 0;
	        ResetAdTimer();
	    }

	    private void ResetAdTimer()
	    {
	        _lastAdTime = DateTime.Now;
	    }
	}
}
