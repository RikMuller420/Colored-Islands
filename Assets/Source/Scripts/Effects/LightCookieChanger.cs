using SlimeGround.Gameplay.Levels;
using UnityEngine;

namespace SlimeGround.Effects
{
	public class LightCookieChanger : MonoBehaviour
    {
		[SerializeField] private Light _mainLight;
		[SerializeField] private RenderTexture _cookie;

		[SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
		private void OnEnable()
		{
			_levelChangeEventTracker.LevelChanged += UpdateCameraPosition;
		}

		private void OnDisable()
		{
			_levelChangeEventTracker.LevelChanged -= UpdateCameraPosition;
		}

		private void UpdateCameraPosition(ILevelData levelData)
		{
			if (levelData.IsMenuLevel)
			{
				_mainLight.cookie = _cookie;
			}
			else
			{
				_mainLight.cookie = null;
			}
		}
	}
}
