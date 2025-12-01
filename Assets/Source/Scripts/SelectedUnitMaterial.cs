using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SelectedUnitMaterial
{
    private const string WidthName = "_OtlWidth";

    public Material Material { get; }

    private Tween _alphaTween;

    private float _appearDuration = 0.3f;
    private float _maxWidth = 6f;
    private float _minWidth = 0f;

    public SelectedUnitMaterial(Material material)
    {
        Material = material;
        DOTween.Init();
    }

    public void StartSelectionAnimation()
    {
        _alphaTween?.Kill();

        SetWidth(_minWidth);

        _alphaTween = DOTween.To(
            () => Material.GetFloat(WidthName),
            width => { Material.SetFloat(WidthName, width); },
            _maxWidth,
            _appearDuration
        )
        .SetEase(Ease.OutQuad);
    }

    private void SetWidth(float value)
    {
        Material.SetFloat(WidthName, value);
    }
}
