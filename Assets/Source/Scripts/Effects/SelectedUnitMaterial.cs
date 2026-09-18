using DG.Tweening;
using UnityEngine;

namespace SlimeGround.Effects
{
	public class SelectedUnitMaterial
	{
	    private const string WidthName = "_OtlWidth";
		private const float AppearDuration = 0.3f;
		private const float MinWidth = 0f;
		private const float MaxWidth = 6f;

		private Tween _alphaTween;

	    public SelectedUnitMaterial(Material material)
	    {
	        Material = material;
	        DOTween.Init();
	    }

		public Material Material { get; }

		public void StartSelectionAnimation()
	    {
	        _alphaTween?.Kill();

	        SetWidth(MinWidth);

	        _alphaTween = DOTween.To(
	            () => Material.GetFloat(WidthName),
	            width => { Material.SetFloat(WidthName, width); },
	            MaxWidth,
	            AppearDuration
	        )
	        .SetEase(Ease.OutQuad);
	    }

	    private void SetWidth(float value)
	    {
	        Material.SetFloat(WidthName, value);
	    }
	}
}
