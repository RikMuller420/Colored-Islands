using System.Collections.Generic;

public class LeaderboardData
{
    public LeaderboardData(string key, int currentPlayerRank, IEnumerable<LeaderboardPlayerData> players)
    {
        Key = key;
        CurrentPlayerRank = currentPlayerRank;
        Players = players;
    }

    public string Key { get; }
    public int CurrentPlayerRank { get; }
    public IEnumerable<LeaderboardPlayerData> Players { get; }
}
