public class TotalGameScore : GameScoreCalcualtor
{
    public TotalGameScore(GameProgressStorage progressStorage) : base(progressStorage) { }

    public override int Score => ProgressStorage.ScoreAmount;
}
