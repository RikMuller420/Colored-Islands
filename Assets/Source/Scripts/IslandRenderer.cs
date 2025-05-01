using UnityEngine;

public class IslandRenderer
{
    private PaintMaterials _paintMaterials;
    private MeshRenderer _renderer;

    public IslandRenderer(MeshRenderer renderer, PaintMaterials paintMaterials)
    {
        _renderer = renderer;
        _paintMaterials = paintMaterials;
    }

    public void SetPaint(Paint paint)
    {
        foreach (PaintMaterialData materials in _paintMaterials.Materials)
        {
            if (materials.Paint == paint)
            {
                _renderer.sharedMaterial = materials.IslandMaterial;

                return;
            }
        }
    }
}
