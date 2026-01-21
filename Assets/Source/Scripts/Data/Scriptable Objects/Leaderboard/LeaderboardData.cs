using SlimeGround.Menu.Windows.Leaderboard;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Leaderboard
{
	[System.Serializable]
	public class LeaderboardData 
	{
	    [SerializeField] private LeaderboardType _type;
	    [SerializeField] private string _key;

	    public LeaderboardData(LeaderboardType type, string key)
	    {
	        _type = type;
	        _key = key;
	    }

	    public LeaderboardType Type => _type;
	    public string Key => _key;
	}
}
