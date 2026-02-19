using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Extensions.Windows
{
	public class MenuDimmer : Dimmer
	{
	    [SerializeField] private Button _button;

	    private MenuWindow _openedMenu;

	    private void OnEnable()
	    {
	        _button.onClick.AddListener(CloseLinkedWindow);
	    }

	    private void OnDisable()
	    {
	        _button.onClick.RemoveListener(CloseLinkedWindow);
	    }

	    public void Activate(MenuWindow menu, bool isAbleToCloseWindow)
	    {
	        _openedMenu = menu;
	        _button.enabled = isAbleToCloseWindow;
			Activate();
	    }

	    private void CloseLinkedWindow()
	    {
	        _openedMenu.Close();
	    }
	}
}
