using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Paints
{
	[System.Serializable]
	public struct ColorSampleMaterialData
	{
	    [SerializeField] private ColorSample _colorSample;
	    [SerializeField] private Material _islandMaterial;
	    [SerializeField] private Material _unitMaterial;
	    [SerializeField] private Material _selectedUnitMaterial;
	    [SerializeField] private Material _hatMaterial;
	    [SerializeField] private Material _selectedHatMaterial;
	    [SerializeField] private Color _islandPointColor;
	    [SerializeField] private Color _unitUiColor;
	    [SerializeField] private Color _unitUiHatColor;
	    [SerializeField] private string _localizationKey;

	    public ColorSample ColorSample => _colorSample;
	    public Material IslandMaterial => _islandMaterial;
	    public Material UnitMaterial => _unitMaterial;
	    public Material SelectedUnitMaterial => _selectedUnitMaterial;
	    public Material HatMaterial => _hatMaterial;
	    public Material SelectedHatMaterial => _selectedHatMaterial;
	    public Color IslandPointColor => _islandPointColor;
	    public Color UnitUiColor => _unitUiColor;
	    public Color UnitUiHatColor => _unitUiHatColor;
	    public string LocalizationKey => _localizationKey;
	}
}
