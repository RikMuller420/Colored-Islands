using System.Collections.Generic;
using UnityEngine;
using YG.Utils.LB;

public class LeaderboardConverter
{
    public LeaderboardData ConvertFrom(LBData yandexLeaderboard)
    {
        int currentPlayerRank = yandexLeaderboard.currentPlayer.rank;
        IEnumerable<LeaderboardPlayerData> players = FormPlayerCollection(yandexLeaderboard.players);

        return new LeaderboardData(yandexLeaderboard.technoName, currentPlayerRank, players);
    }

    private IEnumerable<LeaderboardPlayerData> FormPlayerCollection(LBPlayerData[] yndexPlayers)
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
