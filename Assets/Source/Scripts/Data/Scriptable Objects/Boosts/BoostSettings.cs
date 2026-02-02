using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Boosts
{
	[CreateAssetMenu(fileName = "BoostSettings", menuName = "Custom/BoostSettings")]
	public class BoostSettings : ScriptableObject
	{
	    [SerializeField] private BoostSettingsData[] _boosts;

	    public IReadOnlyCollection<BoostSettingsData> Boosts => _boosts;
	}
}
