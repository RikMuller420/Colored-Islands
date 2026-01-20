using UnityEngine;

public class LeaderboardInitializer : MonoBehaviour
{
    [SerializeField] private LeaderboardSynchronizer _leaderboardSynchronizer;
    [SerializeField] private LeaderboardWindow _leaderboardWindow;
    [SerializeField] private PlayerDataProvider _playerData;

    public void Initialize(LeaderboardProvider leaderboardProvider, IAuthorizationData authorizationData)
    {

        _leaderboardSynchronizer.Initialize(leaderboardProvider, _playerData);
        _leaderboardWindow.Initialize(leaderboardProvider, authorizationData);
    }
}
