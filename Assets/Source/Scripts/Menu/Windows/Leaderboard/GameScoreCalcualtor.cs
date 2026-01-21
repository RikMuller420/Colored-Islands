using SlimeGround.Data.Saves;

namespace SlimeGround.Menu.Windows.Leaderboard
{
	public abstract class GameScoreCalcualtor
	{
	    protected IPlayerData PlayerData;

	    public GameScoreCalcualtor(IPlayerData playerData)
	    {
	        PlayerData = playerData;
	    }

	    public abstract int Score { get; }
	}
}
