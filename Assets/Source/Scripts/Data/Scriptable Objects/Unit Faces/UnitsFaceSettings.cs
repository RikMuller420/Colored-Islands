using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.UnitFaces
{

	[CreateAssetMenu(fileName = "UnitsFaceSettings", menuName = "Custom/UnitsFaceSettings")]
	public class UnitsFaceSettings : ScriptableObject
	{
	    [SerializeField] private UnitFaceData[] _faces;

	    public IReadOnlyCollection<UnitFaceData> Faces => _faces;
	}

}
