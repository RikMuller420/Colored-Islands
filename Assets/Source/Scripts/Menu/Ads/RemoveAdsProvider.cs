using System;
using SlimeGround.Data.Saves;

namespace SlimeGround.Menu.Ads
{
	public class RemoveAdsProvider
	{
		private readonly PlayerDataProvider _playerData;

		public RemoveAdsProvider(PlayerDataProvider playerData)
	    {
	        _playerData = playerData;
			_playerData.Resources.RemoveAdsStateChanged += OnRemoveAdsStateChanged;
	    }

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
