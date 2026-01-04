using System.Collections.Generic;
using YG.Utils.LB;

public class LeaderboardConverter
{
    public Leaderboard ConvertFrom(LBData yandexLeaderboard)
    {
        int currentPlayerRank = yandexLeaderboard.currentPlayer.rank;
        int currentPlayerScore = yandexLeaderboard.currentPlayer.score;

        IReadOnlyCollection<LeaderboardPlayerData> players = FormPlayerCollection(yandexLeaderboard.players);

        return new Leaderboard(yandexLeaderboard.technoName, currentPlayerRank, currentPlayerScore, players);
    }

    private IReadOnlyCollection<LeaderboardPlayerData> FormPlayerCollection(LBPlayerData[] yndexPlayers)
    {
        List<LeaderboardPlayerData> players = new List<LeaderboardPlayerData>();

        foreach (LBPlayerData yandexPlayer in yndexPlayers)
        {
            var player = new LeaderboardPlayerData
            (
                rank: yandexPlayer.rank,
                name: yandexPlayer.name,
                score: yandexPlayer.score,
                photoLink: yandexPlayer.photo
            );

            players.Add(player);
        }

        return players;
    }

}
