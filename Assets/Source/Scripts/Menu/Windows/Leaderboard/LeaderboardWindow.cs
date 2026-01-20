using System.Collections.Generic;
using UI.TabSystem;
using UnityEngine;

public class LeaderboardWindow : MenuWindow
{
    [SerializeField] private LeaderboardSettings _leaderboardSettings;

    [SerializeField] private TabSwitcher _tabSwitcher;
    [SerializeField] private GameObject _loginHintBacground;
    [SerializeField] private GameObject _loginHintContent;

    [SerializeField] private List<LeaderboardTab> _leaderboardTabs;

    private IAuthorizationData _authorizationData;

    public void Initialize(ILeaderboardReader leaderboardReader, IAuthorizationData authorizationData)
    {
        _authorizationData = authorizationData;

        foreach (LeaderboardTab leaderboardTab in _leaderboardTabs)
        {
            leaderboardTab.Initialize(leaderboardReader, _leaderboardSettings);
        }

        _authorizationData.AuthorizationStatusChanged += OnAuthorizationStatusChanged;
        OnAuthorizationStatusChanged();
    }

    private void OnAuthorizationStatusChanged()
    {
        bool isAuthorized = _authorizationData.IsAuthorized;

        _loginHintBacground.SetActive(!isAuthorized);
        _loginHintContent.SetActive(!isAuthorized);

        if (isAuthorized && IsOpened)
        {
            _tabSwitcher.UpdateActiveTab();
        }
    }

    public override void Open()
    {
        if (IsOpened)
        {
            return;
        }

        base.Open();
        MetricSaver.OpenLeaderboardWindow();
    }
}
