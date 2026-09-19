using System;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Integration.Ads;

namespace SlimeGround.Menu.Ads
{
	public class InterstitialAdOpener
	{
	    private const int AddMinLevelId = 4;
	    private const float AddCooldownSeconds = 30f;
	    private const int LoadsBeforeAdd = 1;

		private readonly LevelChangeEventTracker _levelChangeEventTracker;
		private readonly RemoveAdsProvider _removeAdsProvider;
		private readonly InterstitialAdProvider _interAdProvider;
		private readonly RewardedAdProvider _rewardedAdProvider;

		private int _currentLoadsWithoutAdd = 0;
	    private DateTime _lastAdTime;

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
	        _currentLoadsWithoutAdd++;

	        if (levelData.LevelId < AddMinLevelId)
	        {
	            return;
	        }

	        if (_removeAdsProvider.IsAdsRemoved)
	        {
	            return;
	        }

	        float secondsFromLastAd = (float)(DateTime.Now - _lastAdTime).TotalSeconds;

	        if (secondsFromLastAd < AddCooldownSeconds)
	        {
	            return;
	        }

	        if (_currentLoadsWithoutAdd > LoadsBeforeAdd)
	        {
	            _interAdProvider.ShowAd();
	        }
	    }

	    private void OnInterAdOpened()
	    {
	        _currentLoadsWithoutAdd = 0;
	        ResetAdTimer();
	    }

	    private void ResetAdTimer()
	    {
	        _lastAdTime = DateTime.Now;
	    }
	}
}
