using System.Linq;
using SlimeGround.Data.ScriptableObjects.Weather;
using SlimeGround.Gameplay.Levels;
using UnityEngine;

namespace SlimeGround.Effects.Weather
{ 
	public class WeatherChanger : MonoBehaviour
	{
		[SerializeField] private WeatherSettings _settings;
		[SerializeField] private LevelLoader _levelLoader;

		[SerializeField] private GameObject _rain;
		[SerializeField] private GameObject _fallingStars;
		[SerializeField] private ParticleSystem _wind;
		[SerializeField] private Material _waterMaterial;
		[SerializeField] private Light _sunLight;

		private void OnEnable()
		{
			_levelLoader.LevelChanged += OnLevelChanged;
		}

		private void OnDisable()
		{
			_levelLoader.LevelChanged -= OnLevelChanged;
		}

		private void OnLevelChanged(ILevelData levelData)
		{
			SetWeather(levelData.Weather);
		}

		public void SetWeather(WeatherType type)
		{
			WeatherSettingsData settings = _settings.Weathers.FirstOrDefault(weather => weather.Type == type);

			_rain.SetActive(settings.IsRainActive);
			_fallingStars.SetActive(settings.IsFallingStartsActive);
			_waterMaterial.SetColor("_WaterColor", settings.WaterColor);
			_waterMaterial.SetFloat("_Choppiness", settings.WaterChoppiness);
			_sunLight.intensity = settings.SunIntesivity;
			_sunLight.transform.localEulerAngles = settings.SunRotation;

			ParticleSystem.MainModule wind = _wind.main;
			wind.startSpeed = settings.WindSpeed;
			wind.startColor = settings.WindColor;
		}
	}
}