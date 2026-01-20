using System;

public interface ILeaderboardReader
{
    public event Action<Leaderboard> LeaderboardReceived;

    public void GetLeaderboard(string tableKey);
    public void GetPlayerScore(string tableKey);
}
