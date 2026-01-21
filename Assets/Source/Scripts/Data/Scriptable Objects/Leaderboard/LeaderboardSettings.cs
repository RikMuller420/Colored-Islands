using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SlimeGround.Menu.Windows.Leaderboard;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Leaderboard
{
	[CreateAssetMenu(fileName = "LeaderboardSettings", menuName = "Custom/LeaderboardSettings")]
	public class LeaderboardSettings : ScriptableObject
	{
	    [SerializeField] private LeaderboardData[] _leaderboards;

	    public IReadOnlyCollection<LeaderboardData> Leaderboards => new ReadOnlyCollection<LeaderboardData>(_leaderboards);

	    public string LeaderboardKey(LeaderboardType type)
	    {
	        return _leaderboards.FirstOrDefault(board => board.Type == type).Key;
	    }

	    public LeaderboardType LeaderboardType(string key)
	    {
	        return _leaderboards.FirstOrDefault(board => board.Key == key).Type;
	    }
	}
}
