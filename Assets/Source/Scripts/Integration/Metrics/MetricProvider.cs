using YG;

namespace SlimeGround.Integration.Metrics
{
	public class MetricProvider
	{
		private const string MetricVersion = "1.0";

		public MetricProvider()
	    {
			string className = GetType().Name;
			YG2.MetricaSend(MetricKeys.StartKey, className, MetricVersion);
		}
	}
}
