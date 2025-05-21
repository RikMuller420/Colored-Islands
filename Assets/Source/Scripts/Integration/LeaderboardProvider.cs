using System;
using YG;
using YG.Utils.LB;

public class LeaderboardProvider
{
    private const int QuantityTop = 3;
    private const int QuantityAround = 5;
    private const string PhotoSizeKey = "big";

    private LeaderboardConverter _leaderboardConverter;

    public event Action<LeaderboardData> LeaderboardReceived;

    public LeaderboardProvider()
    {
        _leaderboardConverter = new LeaderboardConverter();
        YG2.onGetLeaderboard += OnGetLeaderboard;
    }

    public void SaveScore(string tableKey, int score)
    {
        YG2.SetLeaderboard(tableKey, score);
    }

    public void GetLeaderboard(string tableKey)
    {
        YG2.GetLeaderboard(tableKey, QuantityTop, QuantityAround, PhotoSizeKey);
    }

    private void OnGetLeaderboard(LBData yandexLeaderboard)
    {
        LeaderboardData leaderboard = _leaderboardConverter.ConvertFrom(yandexLeaderboard);
        LeaderboardReceived?.Invoke(leaderboard);
    }
}
