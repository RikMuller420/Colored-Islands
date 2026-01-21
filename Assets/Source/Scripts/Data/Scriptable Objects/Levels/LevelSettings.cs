using System.Collections.Generic;
using System.Collections.ObjectModel;
using SlimeGround.Gameplay.Islands;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Levels
{
	[CreateAssetMenu(fileName = "LevelSettings", menuName = "Custom/LevelSettings")]
	public class LevelSettings : ScriptableObject
	{
	    [SerializeField] private int _lastTriningLevel;
	    [SerializeField] private LevelSettingsData _mainMenu;
	    [SerializeField] private LevelSettingsData[] _levels;
	    [SerializeField] private BuferIslandInitializer[] _buferIslands;

	    public int LastTrainingLevel => _lastTriningLevel;
	    public LevelSettingsData MainMenuSettings => _mainMenu;
	    public IReadOnlyCollection<LevelSettingsData> Levels => new ReadOnlyCollection<LevelSettingsData>(_levels);
	    public IReadOnlyCollection<BuferIslandInitializer> BuferIslands => new ReadOnlyCollection<BuferIslandInitializer>(_buferIslands);
	}
}
