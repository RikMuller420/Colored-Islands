using GameAnalyticsSDK;

public class MetricProvider
{
    private const string BuildName = "0.1";

    public MetricProvider()
    {
        GameAnalytics.SetBuildAllPlatforms(BuildName);
        GameAnalytics.Initialize();
    }
}

