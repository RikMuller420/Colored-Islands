using System;
using SlimeGround.Integration.Authorization;
using SlimeGround.Menu.Windows.Leaderboard;
using YG;
using YG.Utils.LB;

namespace SlimeGround.Integration.Leaderboards
{
	public class LeaderboardProvider : ILeaderboardReader
	{
	    private const int QuantityTop = 3;
	    private const int QuantityAround = 6;
		private const int QuantityTopNotAutorized = 6;
		private const int QuantityAroundNotAutorized = 0;
		private const string PhotoSizeKey = "small";

		private readonly IAuthorizationData _authorization;
	    private readonly LeaderboardConverter _leaderboardConverter;

		public LeaderboardProvider(IAuthorizationData authorizationData)
	    {
			_authorization = authorizationData;
			_leaderboardConverter = new LeaderboardConverter();
	        YG2.onGetLeaderboard += OnGetLeaderboard;
	    }

		public event Action<Leaderboard> LeaderboardReceived;

		public void Dispose()
		{
			YG2.onGetLeaderboard -= OnGetLeaderboard;
		}

		public void SaveScore(string tableKey, int score)
	    {
	        YG2.SetLeaderboard(tableKey, score);
	    }

	    public void GetLeaderboard(string tableKey)
	    {
			int quantityTop = _authorization.IsAuthorized ? QuantityTop : QuantityTopNotAutorized;
			int quantityAround = _authorization.IsAuthorized ? QuantityAround : QuantityAroundNotAutorized;

			YG2.GetLeaderboard(tableKey, quantityTop, quantityAround, PhotoSizeKey);
	    }

	    public void GetPlayerScore(string tableKey)
	    {
	        YG2.GetLeaderboard(tableKey, 0, 0);
	    }

	    private void OnGetLeaderboard(LBData yandexLeaderboard)
	    {
			Leaderboard leaderboard = _leaderboardConverter.ConvertFrom(yandexLeaderboard);
	        LeaderboardReceived?.Invoke(leaderboard);
	    }
	}
}
