using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSettings", menuName = "Custom/UpgradeSettings")]
public class UpgradeSettings : ScriptableObject
{
    [SerializeField] private UpgradeSettingsData[] _upgrades;

    public IReadOnlyCollection<UpgradeSettingsData> Upgrades => _upgrades;
}