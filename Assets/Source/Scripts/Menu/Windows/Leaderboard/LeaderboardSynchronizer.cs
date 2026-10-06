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
			if (_authorization.IsAuthorized == false)
			{
				return;
			}

			int leaderboardScore = leaderboardData.IsCurrentPlayerListed ?
										leaderboardData.CurrentPlayerScore : 0;

			LeaderboardType type = _leaderboardSettings.GetLeaderboardType(leaderboardData.Key);
			int currentScore = _scoreCalculator.GetScore(type);

			if (currentScore != leaderboardScore)
			{
				_leaderboardProvider.SaveScore(leaderboardData.Key, currentScore);
				PlayerScoreChanged?.Invoke(leaderboardData);
			}
		}
	}
}
