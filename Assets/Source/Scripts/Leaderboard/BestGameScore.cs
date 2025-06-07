using System.Linq;

public class BestGameScore : GameScoreCalcualtor
{
    public BestGameScore(GameProgressStorage progressStorage) : base(progressStorage) { }
    public override int Score => CalculateScore();

    private int CalculateScore()
    {
        return ProgressStorage.Levels
                .Where(level => level.IsDone)
                .Sum(level => level.BestScore);
    }
}
