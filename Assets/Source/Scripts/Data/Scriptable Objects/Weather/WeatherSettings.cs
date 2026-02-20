using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Weather
{
	[CreateAssetMenu(fileName = "WeatherSettings", menuName = "Custom/WeatherSettings")]
	public class WeatherSettings : ScriptableObject
	{
		[SerializeField] private WeatherSettingsData[] _weathers;

		public IReadOnlyCollection<WeatherSettingsData> Weathers => _weathers;
	}
}