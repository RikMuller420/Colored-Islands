using UnityEngine;

public class MetricInitializer : MonoBehaviour
{
    [SerializeField] private PlayerDataProvider _playerData;

    public void Initilize(ILevelData currentLevelData)
    {
        var metricProvider = new MetricProvider();
        var metricSaver = new MetricSaver(currentLevelData, _playerData);
    }
}
