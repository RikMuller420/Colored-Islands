using System.Collections.Generic;

public class LeaderboardScoreCalculator
{
    private Dictionary<LeaderboardType, GameScoreCalcualtor> _scoreCalcualtors;

    public LeaderboardScoreCalculator(GameProgressStorage progressStorage)
    {
        _scoreCalcualtors = new Dictionary<LeaderboardType, GameScoreCalcualtor>()
        {
            { LeaderboardType.TotalGameScore, new TotalGameScore(progressStorage) },
            { LeaderboardType.BestGameScore, new BestGameScore(progressStorage) }
        };
    }

    public int GetScore(LeaderboardType type)
    {
        return _scoreCalcualtors[type].Score;
    }
}
