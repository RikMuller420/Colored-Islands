using System.Collections;
using UnityEngine;

public class LeaderboardSynchronizer : MonoBehaviour
{
    private float _synchronizeInterval = 10;
    private WaitForSeconds _wait;

    private LeaderboardProvider _leaderboardProvider;
    private LeaderboardSettings _leaderboardSettings;
    private LeaderboardScoreCalculator _scoreCalculator;

    public void Initialize(LeaderboardProvider leaderboardProvider, LeaderboardSettings leaderboardSettings,
                            LeaderboardScoreCalculator scoreCalculator)
    {
        _leaderboardProvider = leaderboardProvider;
        _leaderboardSettings = leaderboardSettings;
        _scoreCalculator = scoreCalculator;

        _leaderboardProvider.LeaderboardReceived += SynchronizeLeaderboard;
        enabled = true;
    }

    private void Start()
    {
        _wait = new WaitForSeconds(_synchronizeInterval);
        StartCoroutine(Synchronizing());
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
        if (leaderboardData.Players.Count > 1)
        {
            return;
        }

        LeaderboardType type = _leaderboardSettings.LeaderboardType(leaderboardData.Key);
        int _score = _scoreCalculator.GetScore(type);

        if (_score != leaderboardData.CurrentPlayerScore)
        {
            _leaderboardProvider.SaveScore(leaderboardData.Key, _score);
        }
    }
}
