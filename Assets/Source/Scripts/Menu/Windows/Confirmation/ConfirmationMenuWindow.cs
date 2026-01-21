using System;
using SlimeGround.Menu.Extensions.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Confirmation
{
	public class ConfirmationMenuWindow : MenuWindow
	{
	    [SerializeField] private TextMeshProUGUI _text;
	    [SerializeField] private Button _confirmButton;

	    private Action _confirmedDelegate;

	    private new void OnEnable()
	    {
	        base.OnEnable();
	        _confirmButton.onClick.AddListener(Confirm);
	    }

	    private new void OnDisable()
	    {
	        base.OnDisable();
	        _confirmButton.onClick.RemoveListener(Confirm);
	    }

	    public void Open(string confirmationText, Action confirmed)
	    {
	        if (IsOpened)
	        {
	            return;
	        }

	        _text.text = confirmationText;
	        _confirmedDelegate = confirmed;
	        base.Open();
	    }

	    private void Confirm()
	    {
	        Close();
	        _confirmedDelegate?.Invoke();
	    }
	}
}
