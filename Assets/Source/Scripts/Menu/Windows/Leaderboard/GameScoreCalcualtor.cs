public abstract class GameScoreCalcualtor
{
    protected GameProgressStorage ProgressStorage;

    public GameScoreCalcualtor(GameProgressStorage progressStorage)
    {
        ProgressStorage = progressStorage;
    }

    public abstract int Score { get; }
}
