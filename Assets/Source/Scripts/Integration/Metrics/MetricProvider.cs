using GameAnalyticsSDK;

namespace SlimeGround.Integration.Metrics
{
	public class MetricProvider
	{
	    private const string BuildName = "0.1";

	    public MetricProvider()
	    {
	        GameAnalytics.SetBuildAllPlatforms(BuildName);
	        GameAnalytics.Initialize();
	    }
	}
}
