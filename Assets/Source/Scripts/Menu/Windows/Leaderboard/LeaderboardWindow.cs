using System.Collections.Generic;
using SlimeGround.Data.ScriptableObjects.Leaderboard;
using SlimeGround.Integration.Authorization;
using SlimeGround.Integration.Leaderboards;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Extensions.Windows;
using UI.TabSystem;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Leaderboard
{
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

		public void Dispose()
		{
			foreach (LeaderboardTab leaderboardTab in _leaderboardTabs)
			{
				leaderboardTab.Dispose();
			}

			_authorizationData.AuthorizationStatusChanged -= OnAuthorizationStatusChanged;
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
	}
}
