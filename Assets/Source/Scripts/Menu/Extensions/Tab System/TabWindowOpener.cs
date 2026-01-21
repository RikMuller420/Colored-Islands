using SlimeGround.Menu.Extensions.Windows;
using UI.TabSystem;
using UnityEngine;

namespace SlimeGround.Menu.Extensions.TabSystem
{
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
}
