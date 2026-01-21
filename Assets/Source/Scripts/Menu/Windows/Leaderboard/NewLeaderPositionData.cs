using Lean.Localization;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Leaderboard
{

	[System.Serializable]
	public class NewLeaderPositionData
	{
	    [SerializeField] private LeaderboardType _type;
	    [SerializeField] private LeanToken _token;
	    [SerializeField] private string _leanTextKey;

	    public LeaderboardType Type => _type;
	    public LeanToken Token => _token;
	    public string LeanTextKey => _leanTextKey;
	}

}
