using UI.TabSystem;
using UnityEngine;

public class TabWindowOpener : MenuWindowOpener
{
    [SerializeField] private TabSwitcher _tabSwitcher;
    [SerializeField] private int tabIndex;

    protected override void Open()
    {
        base.Open();
        _tabSwitcher.ActivateTab(tabIndex);
    }
}
