using SlimeGround.Data;
using SlimeGround.Menu.Windows.Customization;
using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	public class UnitRenderer : MonoBehaviour
	{
	    [SerializeField] private SkinnedMeshRenderer _renderer;
	    [SerializeField] private Transform _hatHolder;
	    [SerializeField] private TrailRenderer _trail;

	    private Hat _hat;
	    private CustomizationSettingsHolder _customizationSettings;
	    private UnitCustomizationSettings _unitCustomizationSettings;

	    public void Initialize(CustomizationSettingsHolder customizationSettings)
	    {
	        _customizationSettings = customizationSettings;
		}

		public void UpdateShadowCastingMode()
		{
			_renderer.shadowCastingMode = _customizationSettings.ShadowCastingMode;

			if (_hat != null)
			{
				_hat.MeshRenderer.shadowCastingMode = _customizationSettings.ShadowCastingMode;
			}
		}

		public void SetPaint(UnitSlotType slot)
	    {
	        _unitCustomizationSettings = _customizationSettings.GetCustomizationSettings(slot);

	        _renderer.sharedMaterial = _unitCustomizationSettings.UnitMaterial;
	        UpdateHat(_unitCustomizationSettings);
	        _trail.startColor = _unitCustomizationSettings.TrailColor;
	    }

	    public void ActivateOutline()
	    {
	        _renderer.sharedMaterial = _unitCustomizationSettings.SelectedUnitMaterial.Material;
	        _unitCustomizationSettings.SelectedUnitMaterial.StartSelectionAnimation();

	        if (_hat != null)
	        {
				SetHatMaterial(_unitCustomizationSettings.SelectedHatMaterials[_hat.TextureType]);
	        }
	    }

	    public void DeactivateOutline()
	    {
	        _renderer.sharedMaterial = _unitCustomizationSettings.UnitMaterial;

	        if (_hat != null)
	        {
				SetHatMaterial(_unitCustomizationSettings.HatMaterials[_hat.TextureType]);
	        }
	    }

		private void UpdateHat(UnitCustomizationSettings customizationSettings)
		{
			if (_hat != null)
			{
				Destroy(_hat.GameObject);
				_hat = null;
			}

			if (customizationSettings.IsHatEquiped == false)
			{
				return;
			}

			_hat = Instantiate(customizationSettings.HatData.Prefab, _hatHolder);
			SetHatMaterial(customizationSettings.HatMaterials[_hat.TextureType]);
			UpdateShadowCastingMode();
		}

		private void SetHatMaterial(Material material)
		{
			Material[] materials = _hat.MeshRenderer.sharedMaterials;
			materials[0] = material;
			_hat.MeshRenderer.sharedMaterials = materials;
		}

#if UNITY_EDITOR
		public void SetMaterial(Material material)
	    {
	        _renderer.sharedMaterial = material;
	    }
#endif
	}
}
