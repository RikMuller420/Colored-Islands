using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private List<PlayerResultView> _topPlayerViews = new();
    [SerializeField] private List<PlayerResultView> _aroundPlayerViews = new();

    private int _topResulCount;
    private int _allResultsCount;

    private void Awake()
    {
        _topResulCount = _topPlayerViews.Count;
        _allResultsCount = _topPlayerViews.Count + _aroundPlayerViews.Count;
    }

    public void UpdateLeaderboard(Leaderboard leaderboardData)
    {
        IReadOnlyList<LeaderboardPlayerData> topPlayers = GetTopPlayers(leaderboardData);
        UpdatePlayerViews(_topPlayerViews, topPlayers, leaderboardData.CurrentPlayerRank);

        IReadOnlyList<LeaderboardPlayerData> aroundPlayers = GetAroundPlayers(leaderboardData);
        UpdatePlayerViews(_aroundPlayerViews, aroundPlayers, leaderboardData.CurrentPlayerRank);
    }

    private void UpdatePlayerViews(List<PlayerResultView> playerViews, IReadOnlyList<LeaderboardPlayerData> players,
                                   int currentPlayerRank)
    {
        for (int i = 0; i < playerViews.Count; i++)
        {
            if (players.Count > i)
            {
                bool isCurrentPlayer = players[i].Rank == currentPlayerRank;
                playerViews[i].SetPlayeData(players[i], isCurrentPlayer);
            }
            else
            {
                playerViews[i].SetEmptyPlayerData();
            }
        }
    }

    private IReadOnlyList<LeaderboardPlayerData> GetTopPlayers(Leaderboard leaderboardData)
    {
        return leaderboardData.Players.Where(player => player.Rank <= _topPlayerViews.Count)
                                      .OrderBy(player => player.Rank)
                                      .ToList();
    }

    private IReadOnlyList<LeaderboardPlayerData> GetAroundPlayers(Leaderboard leaderboardData)
    {
        if (leaderboardData.CurrentPlayerRank <= _topPlayerViews.Count)
        {
            return leaderboardData.Players
                        .Where(player => player.Rank > _topResulCount && player.Rank <= _allResultsCount)
                        .OrderBy(player => player.Rank)
                        .ToList();
        }
        else if (leaderboardData.CurrentPlayerRank <= _allResultsCount)
        {
            return leaderboardData.Players
                        .Where(player => player.Rank > _topResulCount && player.Rank <= _allResultsCount)
                        .OrderBy(player => player.Rank)
                        .ToList();
        }
        else
        {
            int maxAroundRank = leaderboardData.CurrentPlayerRank - 2;
            int minAroundRank = leaderboardData.CurrentPlayerRank + 1;

            return leaderboardData.Players
                        .Where(player => player.Rank > maxAroundRank && player.Rank <= minAroundRank)
                        .OrderBy(player => player.Rank)
                        .ToList();
        }
    }
}
