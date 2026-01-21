using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.LevelRewards
{

	[CreateAssetMenu(fileName = "LevelRewardSettings", menuName = "Custom/LevelRewardSettings")]
	public class LevelRewardSettings : ScriptableObject
	{
	    [SerializeField] private LevelRewardData[] _levelRewards;

	    public IReadOnlyCollection<LevelRewardData> LevelRewards => _levelRewards;
	}

}
