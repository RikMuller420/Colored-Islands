using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Hats
{
	[CreateAssetMenu(fileName = "UnitsHatSettings", menuName = "Custom/UnitsHatSettings")]
	public class UnitsHatSettings : ScriptableObject
	{
	    [SerializeField] private int _noHatId;
	    [SerializeField] private UnitHatData[] _hats;
		[SerializeField] private List<HatTexture> _hatTextures;

	    public int NoHatId => _noHatId;
	    public IReadOnlyCollection<UnitHatData> Hats => _hats;
		public IReadOnlyCollection<HatTexture> HatTextures => _hatTextures;
	}
}
