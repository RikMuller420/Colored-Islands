using UnityEngine;

[System.Serializable]
public struct PaintMaterialData
{
    [SerializeField] private Paint _paint;
    [SerializeField] private Material _islandMaterial;
    [SerializeField] private Material _unitMaterial;
    [SerializeField] private Material _hatMaterial;
    [SerializeField] private Color _islandPointColor;
    [SerializeField] private Color _unitUiColor;
    [SerializeField] private Color _unitUiHatColor;

    public Paint Paint => _paint;
    public Material IslandMaterial => _islandMaterial;
    public Material UnitMaterial => _unitMaterial;
    public Material HatMaterial => _hatMaterial;
    public Color IslandPointColor => _islandPointColor;
    public Color UnitUiColor => _unitUiColor;
    public Color UnitUiHatColor => _unitUiHatColor;
}
