using UnityEngine;

public class UnitRenderer
{
    private const string OutlineShaderValueName = "_OtlWidth";

    [SerializeField] private MeshRenderer _renderer;

    private float maxOutlineWidth = 10f;
    private float minOutlineWidth = 0f;

    public UnitRenderer(MeshRenderer renderer)
    {
        _renderer = renderer;
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
