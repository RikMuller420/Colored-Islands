using System.Collections.Generic;
using UnityEngine;

public class DeviceStyleChangeInitializer : MonoBehaviour
{
    [SerializeField] private List<ExitButtonStyleChanger> _exitButtonStyleChangers = new();

    public void Initialize()
    {
        var deviceInfoProvider = new DeviceInfoProvider();
        DeviceType deviceType = deviceInfoProvider.GetDeviceType();

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
