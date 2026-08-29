using YG;

namespace SlimeGround.Integration.Metrics
{
	public class MetricProvider
	{
		private const string EventName = "LevelStarted";
		private const string MetricVersion = "0.2";

		public MetricProvider()
	    {
			string className = GetType().Name;
			YG2.MetricaSend(EventName, className, MetricVersion);
		}
	}
}
