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

	    public string GetLeaderboardKey(LeaderboardType type)
	    {
	        return _leaderboards.FirstOrDefault(board => board.Type == type).Key;
	    }

	    public LeaderboardType GetLeaderboardType(string key)
	    {
	        return _leaderboards.FirstOrDefault(board => board.Key == key).Type;
	    }
	}
}
