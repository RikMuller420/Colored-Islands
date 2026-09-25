using System.Collections.Generic;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class Leaderboard
	{
	    public Leaderboard(string key, bool isCurrentPlayerListed,
						   int currentPlayerRank, int currentPlayerScore,
						   IReadOnlyCollection<LeaderboardPlayerData> players)
	    {
	        Key = key;
			IsCurrentPlayerListed = isCurrentPlayerListed;
			CurrentPlayerRank = currentPlayerRank;
			CurrentPlayerScore = currentPlayerScore;
			Players = players;
	    }

	    public string Key { get; }
		public bool IsCurrentPlayerListed { get; }
		public int CurrentPlayerRank { get; }
		public int CurrentPlayerScore { get; }
		public IReadOnlyCollection<LeaderboardPlayerData> Players { get; }
	}
}
