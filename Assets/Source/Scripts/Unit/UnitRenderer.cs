using UnityEngine;

public class UnitRenderer : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Transform _hatHolder;

    private Hat _hat;
    private CustomizationSettingsHolder _customizationSettings;
    private UnitCustomizationSettings _unitCustomizationSettings;

    public void Initialize(CustomizationSettingsHolder customizationSettings)
    {
        _customizationSettings = customizationSettings;
    }

    public void SetPaint(Paint paint)
    {
        _unitCustomizationSettings = _customizationSettings.GetCustomizationSettings(paint);
        _renderer.sharedMaterial = _unitCustomizationSettings.UnitMaterial;
        UpdateHat(_unitCustomizationSettings);
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
        _hat.MeshRenderer.sharedMaterial = customizationSettings.HatMaterial;
    }

    public void ActivateOutline()
    {
        _renderer.sharedMaterial = _unitCustomizationSettings.SelectedUnitMaterial;

        if (_hat != null)
        {
            _hat.MeshRenderer.sharedMaterial = _unitCustomizationSettings.SelectedHatMaterial;
        }
    }

    public void DeactivateOutline()
    {
        _renderer.sharedMaterial = _unitCustomizationSettings.UnitMaterial;

        if (_hat != null)
        {
            _hat.MeshRenderer.sharedMaterial = _unitCustomizationSettings.HatMaterial;
        }
    }

#if UNITY_EDITOR
    public void SetMaterial(Material material)
    {
        _renderer.sharedMaterial = material;
    }
#endif
}
