using GameAnalyticsSDK;

public class MetricProvider
{
    private const string BuildName = "0.1";

    private GameProgressStorage _progressStorage;

    public MetricProvider(GameProgressStorage progressStorage)
    {
        _progressStorage = progressStorage;

        GameAnalytics.SetBuildAllPlatforms(BuildName);
        GameAnalytics.Initialize();
    }
}

