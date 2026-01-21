using System.Collections.Generic;
using SlimeGround.Data.Saves;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class PlayerScoreCalculator
	{
	    private Dictionary<LeaderboardType, GameScoreCalcualtor> _scoreCalcualtors;

	    public PlayerScoreCalculator(IPlayerData playerData)
	    {
	        _scoreCalcualtors = new Dictionary<LeaderboardType, GameScoreCalcualtor>()
	        {
	            { LeaderboardType.TotalGameScore, new TotalGameScore(playerData) },
	            { LeaderboardType.BestGameScore, new BestGameScore(playerData) }
	        };
	    }

	    public int GetScore(LeaderboardType type)
	    {
	        return _scoreCalcualtors[type].Score;
	    }
	}
}
