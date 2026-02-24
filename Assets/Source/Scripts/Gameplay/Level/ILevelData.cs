using System.Collections.Generic;
using SlimeGround.Core.CameraSystem;
using SlimeGround.Effects.Weather;
using SlimeGround.Gameplay.Islands;
using UnityEngine;

namespace SlimeGround.Gameplay.Levels
{
	public interface ILevelData
	{
	    public Transform IslandsParent { get; }
	    public IEnumerable<Island> Islands { get; }
	    public CameraTargets VerticalCameraTargets { get; }
	    public CameraTargets HorizontalCameraTargets { get; }
		public WeatherType Weather { get; }
		public MeshRenderer LevelBounds { get; }

	    public int LevelId { get; }
		public bool IsMenuLevel { get; }
		public int ExtraStarMoveCount { get; }
	    public float ExtraScoreTime { get; }
	    public int BuferIslandSize { get; }
	    public float AngryBarSpeed { get; }
	}
}
