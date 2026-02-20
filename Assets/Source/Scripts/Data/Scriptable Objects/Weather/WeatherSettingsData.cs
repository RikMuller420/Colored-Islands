using SlimeGround.Effects.Weather;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Weather
{ 
	[System.Serializable]
	public class WeatherSettingsData
	{
		[SerializeField] private WeatherType _weatherType;
		[SerializeField] private Color _waterColor;
		[SerializeField] private float _waterChoppiness;
		[SerializeField] private bool _isRainActive;
		[SerializeField] private Vector3 _sunRotation;
		[SerializeField] private float _sunIntesivity;
		[SerializeField] private float _windSpeed;
		[SerializeField] private Color _windColor;
		[SerializeField] private bool _isFallingStartsActive;

		public WeatherType Type => _weatherType;
		public Color WaterColor => _waterColor;
		public float WaterChoppiness => _waterChoppiness;
		public bool IsRainActive => _isRainActive;
		public Vector3 SunRotation => _sunRotation;
		public float SunIntesivity => _sunIntesivity;
		public float WindSpeed => _windSpeed;
		public Color WindColor => _windColor;
		public bool IsFallingStartsActive => _isFallingStartsActive;
	}
}
