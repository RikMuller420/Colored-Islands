using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Hats
{

	[CreateAssetMenu(fileName = "UnitsHatSettings", menuName = "Custom/UnitsHatSettings")]
	public class UnitsHatSettings : ScriptableObject
	{
	    [SerializeField] private int _noHatId;
	    [SerializeField] private UnitHatData[] _hats;

	    public int NoHatId => _noHatId;
	    public IReadOnlyCollection<UnitHatData> Hats => _hats;
	}

}
