using System.Collections.Generic;
using SlimeGround.Menu.Extensions.Windows;
using UnityEngine;

namespace SlimeGround.Menu.Windows
{
	public class UIZoneSwitcher : MonoBehaviour
	{
		private const float BoostAppearDelay = 0.5f;

		[SerializeField] private List<ZoneUi> _mainMenuZones;
	    [SerializeField] private List<ZoneUi> _inGameZones;
	    [SerializeField] private List<MenuWindow> _windows;
	    [SerializeField] private ZoneUi _boostButtonsZone;

	    public void SwitchToInGameUI()
	    {
	        CloseZones(_mainMenuZones);
	        OpenZones(_inGameZones);
	        CloseAllWindows();
	        _boostButtonsZone.OpenWithDelay(BoostAppearDelay);
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
}
