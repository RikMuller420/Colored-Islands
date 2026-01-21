using SlimeGround.Gameplay.Levels;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Levels
{
	[System.Serializable]
	public struct LevelSettingsData
	{
	    [SerializeField] private int _id;
	    [SerializeField] private Level _levelPrefab;

	    public LevelSettingsData(int id, Level levelPrefab)
	    {
	        _id = id;
	        _levelPrefab = levelPrefab;
	    }

	    public int Id => _id;
	    public Level LevelPrefab => _levelPrefab;
	}
}
