using UnityEngine;

public class UnitRenderer
{
    private const string OutlineShaderValueName = "_OtlWidth";

    private MeshRenderer _renderer;
    private PaintMaterials _paintMaterials;

    private float maxOutlineWidth = 10f;
    private float minOutlineWidth = 0f;

    public UnitRenderer(MeshRenderer renderer, PaintMaterials paintMaterials)
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
                _renderer.sharedMaterial = materials.UnitMaterial;

                return;
            }
        }
    }

    public void ActivateOutline()
    {
        _renderer.material.SetFloat(OutlineShaderValueName, maxOutlineWidth);
    }

    public void DeactivateOutline()
    {
        _renderer.material.SetFloat(OutlineShaderValueName, minOutlineWidth);
    }
}
