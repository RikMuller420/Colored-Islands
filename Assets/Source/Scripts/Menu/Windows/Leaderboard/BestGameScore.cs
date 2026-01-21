using System.Linq;
using SlimeGround.Data.Saves;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class BestGameScore : GameScoreCalcualtor
	{
	    public BestGameScore(IPlayerData playerData) : base(playerData) { }
	    public override int Score => CalculateScore();

	    private int CalculateScore()
	    {
	        return PlayerData.Levels
	                .Where(level => level.IsDone)
	                .Sum(level => level.BestScore);
	    }
	}
}
