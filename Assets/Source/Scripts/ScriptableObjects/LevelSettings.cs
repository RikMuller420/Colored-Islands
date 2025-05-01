using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Custom/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    [SerializeField] private LevelSettingsData[] _levels;

    public IReadOnlyCollection<LevelSettingsData> Levels => new ReadOnlyCollection<LevelSettingsData>(_levels);
}
