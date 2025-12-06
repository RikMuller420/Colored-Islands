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

    public void SetPaint(ColorSample colorSample, IReadOnlyCollection<SpriteRenderer> points)
    {
        Debug.Log("Set Island Paint" + colorSample);

        foreach (PaintMaterialData materials in _paintMaterials.Materials)
        {
            if (materials.ColorSample == colorSample)
            {
                _renderer.sharedMaterial = materials.IslandMaterial;
                Debug.Log("_renderer updated");

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
