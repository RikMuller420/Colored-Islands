using System.Collections.Generic;
using YG.Utils.LB;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class LeaderboardConverter
	{
	    public Leaderboard ConvertFrom(LBData yandexLeaderboard)
	    {
			bool isCurrentPlayerListed = yandexLeaderboard.currentPlayer != null;
			int currentPlayerRank = isCurrentPlayerListed ? yandexLeaderboard.currentPlayer.rank : 0;
			int currentPlayerScore = isCurrentPlayerListed ? yandexLeaderboard.currentPlayer.score : 0;
			IReadOnlyCollection<LeaderboardPlayerData> players = FormPlayerCollection(yandexLeaderboard.players);

	        return new Leaderboard(yandexLeaderboard.technoName, isCurrentPlayerListed,
								   currentPlayerRank, currentPlayerScore, players);
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
}
