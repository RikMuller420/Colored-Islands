using UnityEngine;

[System.Serializable]
public struct PaintMaterialData
{
    [SerializeField] private Paint _paint;
    [SerializeField] private Material _islandMaterial;
    [SerializeField] private Material _unitMaterial;

    public Paint Paint => _paint;
    public Material IslandMaterial => _islandMaterial;
    public Material UnitMaterial => _unitMaterial;

    public PaintMaterialData(Paint paint, Material islandMaterial, Material unitMaterial)
    {
        _paint = paint;
        _islandMaterial = islandMaterial;
        _unitMaterial = unitMaterial;
    }
}
