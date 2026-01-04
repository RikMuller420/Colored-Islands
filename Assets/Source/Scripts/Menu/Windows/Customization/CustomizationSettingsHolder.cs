using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizationSettingsHolder
{
    private const string UnitFaceTextureName = "_OverlayTex";

    private PaintMaterials _paintMaterials;
    private GameProgressStorage _progressStorage;
    private UnitsFaceSettings _faceSettings;
    private UnitsHatSettings _hatSettings;

    private List<UnitCustomizationSettings> _customizationSettings = new();

    public CustomizationSettingsHolder(PaintMaterials paintMaterials, GameProgressStorage progressStorage,
                                    UnitsFaceSettings faceSettings, UnitsHatSettings hatSettings)
    {
        _paintMaterials = paintMaterials;
        _progressStorage = progressStorage;
        _faceSettings = faceSettings;
        _hatSettings = hatSettings;

        foreach (Paint paint in Enum.GetValues(typeof(Paint)))
        {
            _customizationSettings.Add(CreateSettings(paint));
        }

        _progressStorage.CustomizationPreferenceChanged += OnCustomizationPreferenceChanged;
    }

    public UnitCustomizationSettings GetCustomizationSettings(Paint paint)
    {
        return _customizationSettings.FirstOrDefault(settings => settings.Paint == paint);
    }

    private void OnCustomizationPreferenceChanged(Paint paint)
    {
        int settingsIndex = _customizationSettings.FindIndex(settings => settings.Paint == paint);
        _customizationSettings[settingsIndex] = CreateSettings(paint);
    }

    private UnitCustomizationSettings CreateSettings(Paint paint)
    {
        CustomizationPreferences preference = _progressStorage.GetCustomizationPreference(paint);
        int faceId = preference.FaceId;
        int hatId = preference.HatId;
        ColorSample colorSample = preference.ColorSample;

        PaintMaterialData materialData = _paintMaterials.Materials.FirstOrDefault(material => material.ColorSample == colorSample);
        Material unitMaterial = materialData.UnitMaterial;
        Material selectedUnitMaterial = materialData.SelectedUnitMaterial;
        UnitFaceData unitFaceData = _faceSettings.Faces.FirstOrDefault(face => face.Id == faceId);

        PrepareUnitMaterial(unitMaterial, unitFaceData);
        PrepareUnitMaterial(selectedUnitMaterial, unitFaceData);

        bool isHatEquiped = hatId != _hatSettings.NoHatId;
        UnitHatData hatData = isHatEquiped ? _hatSettings.Hats.FirstOrDefault(hat => hat.Id == hatId) : null;

        return new UnitCustomizationSettings
        (
            paint,
            colorSample,
            unitMaterial,
            selectedUnitMaterial,
            hatData,
            materialData.HatMaterial,
            materialData.SelectedHatMaterial,
            materialData.UnitUiColor
        );
    }

    private void PrepareUnitMaterial(Material material, UnitFaceData faceData)
    {
        material.SetTexture(UnitFaceTextureName, faceData.Texture);
        material.SetTextureScale(UnitFaceTextureName, faceData.Tilling);
        material.SetTextureOffset(UnitFaceTextureName, faceData.Offset);
    }
}
