using UI.TabSystem;
using UnityEngine;

public class LeaderboardWindow : MenuWindow
{
    [SerializeField] private TabSwitcher _leaderboardTabs;
    [SerializeField] private GameObject _loginHintBacground;
    [SerializeField] private GameObject _loginHintContent;

    private AuthorizationProvider _authorizationProvider;

    public void Initialize(AuthorizationProvider authorizationProvider)
    {
        _authorizationProvider = authorizationProvider;

        _authorizationProvider.AuthorizationStatusChanged += OnAuthorizationStatusChanged;
        OnAuthorizationStatusChanged();
    }

    private void OnAuthorizationStatusChanged()
    {
        bool isAuthorized = _authorizationProvider.IsAuthorized;

        Debug.Log("isAuthorized");

        _loginHintBacground.SetActive(!isAuthorized);
        _loginHintContent.SetActive(!isAuthorized);

        if (isAuthorized && IsOpened)
        {
            _leaderboardTabs.UpdateActiveTab();
        }
    }
}
