using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : ZoneUi
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private MenuDimmer _menuDimmer;

    public event Action MenuOpened;
    public event Action MenuClosed;

    protected void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
    }

    protected void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    public override void Open()
    {
        Debug.Log("open base");
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
}
