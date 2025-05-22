using UnityEngine;

public class LeaderboardTab : TabContent
{
    [SerializeField] private string _leaderboardKey;
    [SerializeField] private LeaderboardView _leaderboardView;

    private LeaderboardProvider _leaderboardProvider;

    public void Initialize(LeaderboardProvider leaderboardProvider)
    {
        _leaderboardProvider = leaderboardProvider;
        _leaderboardProvider.LeaderboardReceived += OnLeaderboardReceived;
    }

    public override void Activate()
    {
        base.Activate();
        _leaderboardProvider.GetLeaderboard(_leaderboardKey);
    }

    private void OnLeaderboardReceived(LeaderboardData leaderboardData)
    {
        if (leaderboardData.Key != _leaderboardKey)
        {
            return;
        }

        _leaderboardView.UpdateLeaderboard(leaderboardData);
    }
}
