using System;
using SlimeGround.Menu.Windows.Leaderboard;

namespace SlimeGround.Integration.Leaderboards
{
	public interface ILeaderboardReader
	{
	    public event Action<Leaderboard> LeaderboardReceived;

	    public void GetLeaderboard(string tableKey);
	    public void GetPlayerScore(string tableKey);
	}
}
