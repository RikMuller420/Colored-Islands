using System.Collections.Generic;
using SlimeGround.Data.Saves;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.Extensions.Windows;
using UI.TabSystem;
using UnityEngine;

namespace SlimeGround.Menu.Windows.LevelMap
{
	public class LevelsWindow : MenuWindow
	{
		private const int LevelPerTab = 20;

		[SerializeField] private IPlayerData _playerData;
	    [SerializeField] private TabSwitcher _tabSwitcher;

	    [SerializeField] private LevelLoader _levelLoader;
	    [SerializeField] private List<LevelTabInitializer> _levelTabInitializers;

	    public void Initialize(IPlayerData playerData)
	    {
	        _playerData = playerData;

	        foreach (LevelTabInitializer levelTabInitializer in _levelTabInitializers)
	        {
	            levelTabInitializer.InitializeButtons(_playerData, _levelLoader);
	        }
	    }

	    public override void Open()
	    {
	        if (IsOpened)
	        {
	            return;
	        }

	        base.Open();

			int lastLevelId = _playerData.Progress.GetLastAvailableLevelId();
			int tabIndex = (lastLevelId - 1) / LevelPerTab;

	        if (tabIndex < 0)
	        {
	            tabIndex = 0;
	        }

	        _tabSwitcher.ActivateTab(tabIndex);
	    }
	}
}
