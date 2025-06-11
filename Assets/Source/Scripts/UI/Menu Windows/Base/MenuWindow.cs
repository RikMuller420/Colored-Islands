using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : ZoneUi
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private MenuDimmer _menuDimmer;

    public event Action MenuOpened;

    protected void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
    }

    protected void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    public void Open(bool isAbleToCloseWindow = true)
    {
        if (IsOpened)
        {
            return;
        }

        _menuDimmer.Activate(this, isAbleToCloseWindow);
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
    }
}
