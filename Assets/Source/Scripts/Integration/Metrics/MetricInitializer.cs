using SlimeGround.Data.Saves;
using SlimeGround.Gameplay.Levels;
using UnityEngine;

namespace SlimeGround.Integration.Metrics
{
	public class MetricInitializer : MonoBehaviour
	{
	    [SerializeField] private PlayerDataProvider _playerData;

	    public void Initilize(ILevelData currentLevelData)
	    {
	        var metricProvider = new MetricProvider();
	        var metricSaver = new MetricSaver(currentLevelData, _playerData);
	    }
	}
}
