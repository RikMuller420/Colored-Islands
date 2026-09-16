using YG;

namespace SlimeGround.Integration.Metrics
{
	public class MetricProvider
	{
		private const string EventName = "MetricProviderStarted";
		private const string MetricVersion = "1.0";

		public MetricProvider()
	    {
			string className = GetType().Name;
			YG2.MetricaSend(EventName, className, MetricVersion);
		}
	}
}
