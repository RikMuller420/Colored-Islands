using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizationSettingsHolder
{
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

        foreach (PaintMaterialData paint in _paintMaterials.Materials)
        {
            _customizationSettings.Add(CreateSettings(paint.Paint));
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
        int faceId = _progressStorage.GetCustomizationPreference(paint).FaceId;
        int hatId = _progressStorage.GetCustomizationPreference(paint).HatId;

        Material unitMaterial = _paintMaterials.Materials.FirstOrDefault(material => material.Paint == paint).UnitMaterial;
        Material faceMaterial = _faceSettings.Faces.FirstOrDefault(face => face.Id == faceId).Material;
        Material[] unitMaterials = new Material[] { unitMaterial , faceMaterial };

        bool isHatEquiped = hatId != _hatSettings.NoHatId;
        UnitHatData hatData = isHatEquiped ? _hatSettings.Hats.FirstOrDefault(hat => hat.Id == hatId) : null;
        Material hatMaterial = _paintMaterials.Materials.FirstOrDefault(material => material.Paint == paint).HatMaterial;

        return new UnitCustomizationSettings
        (
            paint,
            unitMaterials,
            hatData,
            hatMaterial
        );
    }
}
