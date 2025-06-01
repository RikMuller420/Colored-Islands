using UnityEngine;

public class UnitRenderer
{
    private const string OutlineShaderValueName = "_OtlWidth";

    private SkinnedMeshRenderer _renderer;
    private PaintMaterials _paintMaterials;

    private float maxOutlineWidth = 6f;
    private float minOutlineWidth = 0f;

    public UnitRenderer(SkinnedMeshRenderer renderer, PaintMaterials paintMaterials)
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
                Material[] sharedMaterials = _renderer.sharedMaterials;
                sharedMaterials[0] = materials.UnitMaterial;
                _renderer.materials = sharedMaterials;

                return;
            }
        }
    }

    public void ActivateOutline()
    {
        _renderer.materials[0].SetFloat(OutlineShaderValueName, maxOutlineWidth);
    }

    public void DeactivateOutline()
    {
        _renderer.materials[0].SetFloat(OutlineShaderValueName, minOutlineWidth);
    }
}
