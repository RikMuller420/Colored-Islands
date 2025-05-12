using UnityEngine;
using YG;

public class LeadersMenuButton : MenuWindowOpener
{
    [SerializeField] private LeaderboardYG _leaderboard;

    protected override void Open()
    {
        _leaderboard.UpdateLB();
        base.Open();
    }
}
