using UnityEngine;

public class LeadersMenuButton : MenuWindowOpener
{
    private LeaderboardProvider _leaderboardProvide;

    public void Initialized(LeaderboardProvider leaderboardProvider)
    {
        _leaderboardProvide = leaderboardProvider;
    }

    protected override void Open()
    {
        //_leaderboard.UpdateLB();
        base.Open();
    }
}
