using System.Collections.Generic;
using SlimeGround.Integration.DeviceInfo;
using UnityEngine;

namespace SlimeGround.Menu.Extensions.DeviceStyle
{

	public class DeviceStyleChangeInitializer : MonoBehaviour
	{
	    [SerializeField] private List<ExitButtonStyleChanger> _exitButtonStyleChangers = new();

	    public void Initialize()
	    {
	        var deviceInfoProvider = new DeviceInfoProvider();
	        Integration.DeviceInfo.DeviceType deviceType = deviceInfoProvider.GetDeviceType();

	        foreach (IDeviceStyleChanger styleChanger in _exitButtonStyleChangers)
	        {
	            styleChanger.SetStyle(deviceType);
	        }
	    }

	    [ContextMenu("Fill StyleChangers List")]
	    public void FillStyleChangersList()
	    {
	        ExitButtonStyleChanger[] foundChangers = FindObjectsOfType<ExitButtonStyleChanger>(true);

	        _exitButtonStyleChangers.Clear();
	        _exitButtonStyleChangers.AddRange(foundChangers);
	    }
	}

}
