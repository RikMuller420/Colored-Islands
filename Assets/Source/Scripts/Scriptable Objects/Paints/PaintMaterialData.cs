using UnityEngine;

[System.Serializable]
public struct PaintMaterialData
{
    [SerializeField] private Paint _paint;
    [SerializeField] private Material _islandMaterial;
    [SerializeField] private Material _unitMaterial;
    [SerializeField] private Color _islandPointColor;

    public PaintMaterialData(Paint paint, Material islandMaterial, Material unitMaterial,
                            Color islandPointColor)
    {
        _paint = paint;
        _islandMaterial = islandMaterial;
        _unitMaterial = unitMaterial;
        _islandPointColor = islandPointColor;
    }

    public Paint Paint => _paint;
    public Material IslandMaterial => _islandMaterial;
    public Material UnitMaterial => _unitMaterial;
    public Color IslandPointColor => _islandPointColor;
}
