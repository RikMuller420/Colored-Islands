using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Extensions.Windows
{
	public class MenuWindow : ZoneUi
	{
	    [SerializeField] private Button _closeButton;
	    [SerializeField] private MenuDimmer _menuDimmer;

	    public event Action MenuOpened;
	    public event Action MenuClosed;

	    protected virtual void OnEnable()
	    {
	        _closeButton?.onClick.AddListener(Close);
	    }

	    protected virtual void OnDisable()
	    {
	        _closeButton?.onClick.RemoveListener(Close);
	    }

	    public override void Open()
	    {
	        if (IsOpened)
	        {
	            return;
	        }

	        _menuDimmer.Activate(this, true);
	        base.Open();
	        MenuOpened?.Invoke();
	    }

	    public void OpenUnclosableWindow()
	    {
	        if (IsOpened)
	        {
	            return;
	        }

	        _menuDimmer.Activate(this, false);
	        base.Open();
	        MenuOpened?.Invoke();
	    }

	    public override void Close()
	    {
	        if (IsOpened == false)
	        {
	            return;
	        }

	        _menuDimmer.Deactivate();
	        base.Close();
	        MenuClosed?.Invoke();
	    }

	    public void EnableCloseButtons()
	    {
	        _menuDimmer.Activate(this, true);
	        _closeButton.interactable = true;
	    }

	    public void DisableCloseButtons()
	    {
	        _menuDimmer.Activate(this, false);
	        _closeButton.interactable = false;
	    }
	}
}
