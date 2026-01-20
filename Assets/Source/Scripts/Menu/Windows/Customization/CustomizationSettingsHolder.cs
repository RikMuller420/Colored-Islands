using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizationSettingsHolder
{
    private const string UnitFaceTextureName = "_OverlayTex";

    private PaintMaterials _paintMaterials;
    private IPlayerData _playerData;
    private UnitsFaceSettings _faceSettings;
    private UnitsHatSettings _hatSettings;

    private List<UnitCustomizationSettings> _customizationSettings = new();

    public CustomizationSettingsHolder(PaintMaterials paintMaterials, IPlayerData playerData,
                                    UnitsFaceSettings faceSettings, UnitsHatSettings hatSettings)
    {
        _paintMaterials = paintMaterials;
        _playerData = playerData;
        _faceSettings = faceSettings;
        _hatSettings = hatSettings;

        foreach (UnitSlotType slot in Enum.GetValues(typeof(UnitSlotType)))
        {
            _customizationSettings.Add(CreateSettings(slot));
        }

        _playerData.CustomizationPreferenceChanged += OnCustomizationPreferenceChanged;
    }

    public UnitCustomizationSettings GetCustomizationSettings(UnitSlotType slot)
    {
        return _customizationSettings.FirstOrDefault(settings => settings.Slot == slot);
    }

    private void OnCustomizationPreferenceChanged(UnitSlotType slot)
    {
        int settingsIndex = _customizationSettings.FindIndex(settings => settings.Slot == slot);
        _customizationSettings[settingsIndex] = CreateSettings(slot);
    }

    private UnitCustomizationSettings CreateSettings(UnitSlotType slot)
    {
        CustomizationPreferences preference = _playerData.GetCustomizationPreference(slot);
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
            slot,
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
