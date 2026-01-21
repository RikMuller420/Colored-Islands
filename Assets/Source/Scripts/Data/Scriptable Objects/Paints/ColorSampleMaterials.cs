using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Paints
{
	[CreateAssetMenu(fileName = "ColorSampleMaterials", menuName = "Custom/ColorSampleMaterials")]
	public class ColorSampleMaterials : ScriptableObject
	{
	    [SerializeField] private ColorSampleMaterialData[] _materials;

	    public IReadOnlyCollection<ColorSampleMaterialData> Materials => new ReadOnlyCollection<ColorSampleMaterialData>(_materials);
	}
}
