using System;
using System.Collections;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Leaderboard;
using SlimeGround.Integration.Authorization;
using SlimeGround.Integration.Leaderboards;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class LeaderboardSynchronizer : MonoBehaviour
	{
		private const float SynchronizeInterval = 10;

		[SerializeField] private LeaderboardSettings _leaderboardSettings;

		private WaitForSeconds _wait;
		private LeaderboardProvider _leaderboardProvider;
		private IAuthorizationData _authorization;
		private PlayerScoreCalculator _scoreCalculator;

		public event Action<Leaderboard> PlayerScoreChanged;

		private void Start()
		{
			_wait = new WaitForSeconds(SynchronizeInterval);
			StartCoroutine(Synchronizing());
		}

		public void Initialize(LeaderboardProvider leaderboardProvider, IPlayerData playerData,
							   IAuthorizationData authorizationData)
		{
			_leaderboardProvider = leaderboardProvider;
			_scoreCalculator = new PlayerScoreCalculator(playerData);
			_authorization = authorizationData;

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
					if (_authorization.IsAuthorized)
					{
						_leaderboardProvider.GetPlayerScore(leaderboardData.Key);
					}

					yield return _wait;
				}
			}
		}

		private void SynchronizeLeaderboard(Leaderboard leaderboardData)
		{
			if (leaderboardData.IsCurrentPlayerListed == false)
			{
				return;
			}

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
