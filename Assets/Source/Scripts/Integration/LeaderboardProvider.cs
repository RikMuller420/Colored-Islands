using System;
using YG;
using YG.Utils.LB;

public class LeaderboardProvider
{
    private const int QuantityTop = 3;
    private const int QuantityAround = 6;
    private const string PhotoSizeKey = "small";

    private LeaderboardConverter _leaderboardConverter;

    public event Action<Leaderboard> LeaderboardReceived;

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

    public void GetPlayerScore(string tableKey)
    {
        YG2.GetLeaderboard(tableKey, 0, 0);
    }

    private void OnGetLeaderboard(LBData yandexLeaderboard)
    {
        Leaderboard leaderboard = _leaderboardConverter.ConvertFrom(yandexLeaderboard);
        LeaderboardReceived?.Invoke(leaderboard);
    }
}
