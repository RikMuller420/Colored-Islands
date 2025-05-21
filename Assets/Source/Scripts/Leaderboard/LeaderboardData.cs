using System.Collections.Generic;

public class LeaderboardData
{
    public LeaderboardData(string tableKey, int currentPlayerRank, IEnumerable<LeaderboardPlayerData> players)
    {
        TableKey = tableKey;
        CurrentPlayerRank = currentPlayerRank;
        Players = players;
    }

    public string TableKey { get; }
    public int CurrentPlayerRank { get; }
    public IEnumerable<LeaderboardPlayerData> Players { get; }
}
