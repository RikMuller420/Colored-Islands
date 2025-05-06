using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : ZoneUi
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private MenuDimmer _menuDimmer;

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
        if (IsOpened)
        {
            return;
        }

        _menuDimmer.Activate();
        base.Open();
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
