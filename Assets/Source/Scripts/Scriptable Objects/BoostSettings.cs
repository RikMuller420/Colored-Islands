using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


[CreateAssetMenu(fileName = "BoostSettings", menuName = "Custom/BoostSettings")]
public class BoostSettings : ScriptableObject
{
    [SerializeField] private BoostSettingsData[] _boosts;

    public IReadOnlyCollection<BoostSettingsData> Boosts => _boosts;
}

