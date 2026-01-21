using SlimeGround.Data.Saves;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public class TotalGameScore : GameScoreCalcualtor
	{
	    public TotalGameScore(IPlayerData playerData) : base(playerData) { }

	    public override int Score => PlayerData.ScoreAmount;
	}
}
