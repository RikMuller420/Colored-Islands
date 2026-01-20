using System.Collections.Generic;
using UI.TabSystem;
using UnityEngine;
using static YG.YG2;

public class LevelsWindow : MenuWindow
{
    [SerializeField] private IPlayerData _playerData;
    [SerializeField] private TabSwitcher _tabSwitcher;

    private int _levelPerTab = 20;

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

        int tabIndex = (_playerData.LastAvailableLevelId - 1) / _levelPerTab;

        if (tabIndex < 0)
        {
            tabIndex = 0;
        }

        _tabSwitcher.ActivateTab(tabIndex);
    }
}
