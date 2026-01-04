using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InAppSettings", menuName = "Custom/InAppSettings")]
public class InAppSettings : ScriptableObject
{
    [SerializeField] private InAppSettingsData[] _inApps;

    public IEnumerable<InAppSettingsData> InApps => _inApps;
}
