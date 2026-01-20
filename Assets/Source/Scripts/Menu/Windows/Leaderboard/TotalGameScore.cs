public class TotalGameScore : GameScoreCalcualtor
{
    public TotalGameScore(IPlayerData playerData) : base(playerData) { }

    public override int Score => PlayerData.ScoreAmount;
}
