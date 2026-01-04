using System.Collections.Generic;
using UnityEngine;

public class UIZoneSwitcher : MonoBehaviour
{
    [SerializeField] private List<ZoneUi> _mainMenuZones;
    [SerializeField] private List<ZoneUi> _inGameZones;
    [SerializeField] private List<MenuWindow> _windows;
    [SerializeField] private ZoneUi _boostButtonsZone;

    private float _boostAppearDelay = 0.5f;

    public void SwitchToInGameUI()
    {
        CloseZones(_mainMenuZones);
        OpenZones(_inGameZones);
        CloseAllWindows();
        _boostButtonsZone.OpenWithDelay(_boostAppearDelay);
    }

    public void SwitchToMainMenuUI()
    {
        CloseZones(_inGameZones);
        OpenZones(_mainMenuZones);
        CloseAllWindows();
        _boostButtonsZone.Close();
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
