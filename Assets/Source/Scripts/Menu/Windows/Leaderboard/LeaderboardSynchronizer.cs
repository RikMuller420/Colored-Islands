using System;
using System.Collections;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Leaderboard;
using SlimeGround.Integration.Leaderboards;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class LeaderboardSynchronizer : MonoBehaviour
	{
	    [SerializeField] private LeaderboardSettings _leaderboardSettings;

	    private float _synchronizeInterval = 15;
	    private WaitForSeconds _wait;

	    private LeaderboardProvider _leaderboardProvider;
	    private PlayerScoreCalculator _scoreCalculator;

	    public event Action<Leaderboard> PlayerScoreChanged;

	    private void Start()
	    {
	        _wait = new WaitForSeconds(_synchronizeInterval);
	        StartCoroutine(Synchronizing());
	    }

	    public void Initialize(LeaderboardProvider leaderboardProvider, IPlayerData playerData)
	    {
	        _leaderboardProvider = leaderboardProvider;
	        _scoreCalculator = new PlayerScoreCalculator(playerData);

	        _leaderboardProvider.LeaderboardReceived += SynchronizeLeaderboard;
	        enabled = true;
	    }

		public void Dispose()
		{
			_leaderboardProvider.LeaderboardReceived -= SynchronizeLeaderboard;
		}

		private IEnumerator Synchronizing()
	    {
	        while (enabled)
	        {
	            foreach (LeaderboardData leaderboardData in _leaderboardSettings.Leaderboards)
	            {
	                _leaderboardProvider.GetPlayerScore(leaderboardData.Key);

	                yield return _wait;
	            }
	        }
	    }

	    private void SynchronizeLeaderboard(Leaderboard leaderboardData)
	    {
	        LeaderboardType type = _leaderboardSettings.LeaderboardType(leaderboardData.Key);
	        int score = _scoreCalculator.GetScore(type);

	        if (score != leaderboardData.CurrentPlayerScore)
	        {
	            _leaderboardProvider.SaveScore(leaderboardData.Key, score);
	            PlayerScoreChanged?.Invoke(leaderboardData);
	        }
	    }
	}
}
