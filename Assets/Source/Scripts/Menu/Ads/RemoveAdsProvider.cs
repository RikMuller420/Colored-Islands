using System;
using SlimeGround.Data.Saves;

namespace SlimeGround.Menu.Ads
{
	public class RemoveAdsProvider
	{
	    private PlayerDataProvider _playerData;

	    public RemoveAdsProvider(PlayerDataProvider playerData)
	    {
	        _playerData = playerData;
	        playerData.RemoveAdsStateChanged += OnRemoveAdsStateChanged;
	    }

		public event Action RemoveAdsStateChanged;

		public bool IsAdsRemoved => _playerData.IsAdsRemoved;

	    public void RemoveAds()
	    {
	        _playerData.ApplyRemoveAddBonus();
	        _playerData.Save();
	    }

	    private void OnRemoveAdsStateChanged()
	    {
	        RemoveAdsStateChanged?.Invoke();
	    }
	}
}
