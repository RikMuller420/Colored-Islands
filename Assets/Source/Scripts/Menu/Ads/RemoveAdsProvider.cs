using System;
using SlimeGround.Data.Saves;

namespace SlimeGround.Menu.Ads
{
	public class RemoveAdsProvider
	{
		public RemoveAdsProvider(PlayerDataProvider playerData)
	    {
	        _playerData = playerData;
			_playerData.Resources.RemoveAdsStateChanged += OnRemoveAdsStateChanged;
	    }

		private PlayerDataProvider _playerData { get; }

		public event Action RemoveAdsStateChanged;

		public bool IsAdsRemoved => _playerData.Resources.IsAdsRemoved;

		public void Dispose()
		{
			_playerData.Resources.RemoveAdsStateChanged -= OnRemoveAdsStateChanged;
		}

	    public void RemoveAds()
	    {
	        _playerData.Resources.ApplyRemoveAddBonus();
	        _playerData.Save();
	    }

	    private void OnRemoveAdsStateChanged()
	    {
	        RemoveAdsStateChanged?.Invoke();
	    }
	}
}
