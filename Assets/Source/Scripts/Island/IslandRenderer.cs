using System.Collections.Generic;
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

    public void SetPaint(Paint paint, IReadOnlyCollection<SpriteRenderer> points)
    {
        foreach (PaintMaterialData materials in _paintMaterials.Materials)
        {
            if (materials.Paint == paint)
            {
                _renderer.sharedMaterial = materials.IslandMaterial;
                SetColor(materials.IslandPointColor, points);

                return;
            }
        }
    }

    private void SetColor(Color color, IReadOnlyCollection<SpriteRenderer> points)
    {
        foreach (SpriteRenderer islandPoint in points)
        {
            islandPoint.color = color;
        }
    }
}
