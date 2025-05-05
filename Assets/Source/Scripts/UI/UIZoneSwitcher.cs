using System.Collections.Generic;
using UnityEngine;

public class UIZoneSwitcher : MonoBehaviour
{
    [SerializeField] private List<ZoneUi> _mainMenuZones;
    [SerializeField] private List<ZoneUi> _inGameZones;
    [SerializeField] private List<MenuWindow> _windows;

    public void SwitchToInGameUI()
    {
        OpenZones(_inGameZones);
        CloseZones(_mainMenuZones);
        CloseAllWindows();
    }

    public void SwitchToMainMenuUI()
    {
        OpenZones(_mainMenuZones);
        CloseZones(_inGameZones);
        CloseAllWindows();
    }

    private void OpenZones(IEnumerable<ZoneUi> zones)
    {
        foreach (ZoneUi zone in zones)
        {
            zone.Open();
        }
    }

    private void CloseZones(IEnumerable<ZoneUi> zones)
    {
        foreach (ZoneUi zone in zones)
        {
            zone.Close();
        }
    }

    private void CloseAllWindows()
    {
        foreach (MenuWindow window in _windows)
        {
            window.Close();
        }
    }
}
