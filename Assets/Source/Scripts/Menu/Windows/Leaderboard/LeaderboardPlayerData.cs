
namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class LeaderboardPlayerData
	{
	    public LeaderboardPlayerData(int rank, string name, int score, string photoLink = null)
	    {
	        Rank = rank;
	        Name = name;
	        Score = score;
	        PhotoLink = photoLink;
	    }

	    public int Rank { get; }
	    public string Name { get; }
	    public int Score { get; }
	    public string PhotoLink { get; }
	}

}
