using UI.TabSystem;
using UnityEngine;

public class LevelsWindow : MenuWindow
{
    [SerializeField] private TabSwitcher _tabSwitcher;

    private GameProgressStorage _progressStorage;
    private int _levelPerTab = 20;

    public void Initialize(GameProgressStorage progressStorage)
    {
        _progressStorage = progressStorage;
    }

    public override void Open()
    {
        if (IsOpened)
        {
            return;
        }

        base.Open();

        int tabIndex = (_progressStorage.LastAvailableLevelId - 1) / _levelPerTab;

        if (tabIndex < 0)
        {
            tabIndex = 0;
        }

        _tabSwitcher.ActivateTab(tabIndex);
    }
}
