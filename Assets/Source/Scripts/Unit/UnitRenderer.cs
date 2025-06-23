using System.Linq;
using UnityEngine;

public class UnitRenderer : MonoBehaviour
{
    private const string OutlineShaderValueName = "_OtlWidth";

    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Transform _hatHolder;

    private Hat _hat;
    private CustomizationSettingsHolder _customizationSettings;

    private float maxOutlineWidth = 6f;
    private float minOutlineWidth = 0f;

    public void Initialize(CustomizationSettingsHolder customizationSettings)
    {
        _customizationSettings = customizationSettings;
    }

    public void SetPaint(Paint paint)
    {
        UnitCustomizationSettings customizationSettings = _customizationSettings.GetCustomizationSettings(paint);
        _renderer.sharedMaterials = customizationSettings.UnitMaterials;
        UpdateHat(customizationSettings);
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
        _renderer.materials[0].SetFloat(OutlineShaderValueName, maxOutlineWidth);

        if (_hat != null)
        {
            _hat.MeshRenderer.material.SetFloat(OutlineShaderValueName, maxOutlineWidth);
        }
    }

    public void DeactivateOutline()
    {
        _renderer.materials[0].SetFloat(OutlineShaderValueName, minOutlineWidth);

        if (_hat != null)
        {
            _hat.MeshRenderer.material.SetFloat(OutlineShaderValueName, minOutlineWidth);
        }
    }

#if UNITY_EDITOR
    public void SetMaterial(Material material)
    {
        _renderer.sharedMaterial = material;
    }
#endif
}
