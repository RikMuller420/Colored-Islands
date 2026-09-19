using System.Collections.Generic;
using SlimeGround.Core.CameraSystem;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Effects.Weather;
using SlimeGround.Gameplay.Islands;
using UnityEngine;

namespace SlimeGround.Gameplay.Levels
{
	public class LevelDataHolder : ILevelData
	{
		private readonly LevelSettingsData _menuLevel;

		private LevelSettingsData _currentLevelSettings;

		public LevelDataHolder(LevelSettingsData menuLevel)
		{
			_menuLevel = menuLevel;
			SetLevelData(null, menuLevel);
		}
		
		public Level Level { get; private set; }

	    public Transform IslandsParent => Level?.transform;
	    public IEnumerable<Island> Islands => Level?.Islands;
	    public CameraTargets VerticalCameraTargets => Level?.CameraTargetsVertical;
	    public CameraTargets HorizontalCameraTargets => Level?.CameraTargetsHorizontal;
		public WeatherType Weather => Level == null ? WeatherType.Sun : Level.Weather;
		public MeshRenderer LevelBounds => Level?.LevelBounds;

	    public int LevelId => _currentLevelSettings.Id;
		public bool IsMenuLevel => _menuLevel.Id == _currentLevelSettings.Id;
		public int ExtraStarMoveCount => Level.ExtraStarMoveCount;
	    public float ExtraScoreTime => Level.ExtraScoreTime;
	    public int BuferIslandSize => Level.BuferIslandSize;
	    public float AngryBarSpeed => Level.AngryBarSpeed;

	    public void SetLevelData(Level level, LevelSettingsData levelData)
	    {
	        Level = level;
	        _currentLevelSettings = levelData;
	    }
	}
}
