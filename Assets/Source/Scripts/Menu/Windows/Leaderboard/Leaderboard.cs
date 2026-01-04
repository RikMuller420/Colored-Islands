using System.Collections.Generic;

public class Leaderboard
{
    public Leaderboard(string key, int currentPlayerRank, int currentPlayerScore,
                        IReadOnlyCollection<LeaderboardPlayerData> players)
    {
        Key = key;
        CurrentPlayerRank = currentPlayerRank;
        Players = players;
        CurrentPlayerScore = currentPlayerScore;
    }

    public string Key { get; }
    public int CurrentPlayerRank { get; }
    public int CurrentPlayerScore { get; }
    public IReadOnlyCollection<LeaderboardPlayerData> Players { get; }
}
