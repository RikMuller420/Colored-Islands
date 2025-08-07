using UnityEngine;

public class LeaderboardTab : TabContent
{
    [SerializeField] private LeaderboardType _type;
    [SerializeField] private LeaderboardView _view;

    private LeaderboardProvider _leaderboardProvider;
    private string _leaderboarKey;

    public void Initialize(LeaderboardProvider leaderboardProvider, LeaderboardSettings leaderboardSettings)
    {
        _leaderboardProvider = leaderboardProvider;
        _leaderboarKey = leaderboardSettings.LeaderboardKey(_type);

        _leaderboardProvider.LeaderboardReceived += OnLeaderboardReceived;
    }

    public override void Activate()
    {
        base.Activate();
        _leaderboardProvider.GetLeaderboard(_leaderboarKey);
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
