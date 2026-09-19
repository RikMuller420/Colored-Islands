using System;
using SlimeGround.Data.ScriptableObjects.Leaderboard;
using SlimeGround.Integration.Leaderboards;
using SlimeGround.Menu.Extensions.TabSystem;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class LeaderboardTab : TabContent
	{
		private const float RefreshCooldownSeconds = 20f;

		[SerializeField] private LeaderboardType _type;
	    [SerializeField] private LeaderboardView _view;

	    private ILeaderboardReader _leaderboardReader;
	    private string _leaderboarKey;
	    private DateTime _lastRefreshTime = DateTime.MinValue;

	    public void Initialize(ILeaderboardReader leaderboardReader, LeaderboardSettings leaderboardSettings)
	    {
	        _leaderboardReader = leaderboardReader;
	        _leaderboarKey = leaderboardSettings.LeaderboardKey(_type);

	        _leaderboardReader.LeaderboardReceived += OnLeaderboardReceived;
	    }

		public void Dispose()
		{
			_leaderboardReader.LeaderboardReceived -= OnLeaderboardReceived;
		}

		public override void Activate()
	    {
	        base.Activate();

	        if ((DateTime.Now - _lastRefreshTime).Seconds > RefreshCooldownSeconds)
	        {
	            _leaderboardReader.GetLeaderboard(_leaderboarKey);
	            _lastRefreshTime = DateTime.Now;
	        }
	    }

	    private void OnLeaderboardReceived(Leaderboard leaderboardData)
	    {
	        if (leaderboardData.Key != _leaderboarKey)
	        {
	            return;
	        }

	        _view.UpdateLeaderboard(leaderboardData);
	    }
	}
}
