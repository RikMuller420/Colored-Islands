using UnityEngine;

public class UnitCustomizationSettings
{
    public UnitCustomizationSettings(Paint paint, Material unitMaterial, Material selectedUnitMaterial,
                                    UnitHatData hatData, Material hatMaterial, Material selectedHatMaterial)
    {
        Paint = paint;
        UnitMaterial = unitMaterial;
        SelectedUnitMaterial = selectedUnitMaterial;
        HatData = hatData;
        HatMaterial = hatMaterial;
        SelectedHatMaterial = selectedHatMaterial;
    }

    public Paint Paint { get; }
    public Material UnitMaterial { get; }
    public Material SelectedUnitMaterial { get; }
    public bool IsHatEquiped => HatData != null;
    public UnitHatData HatData { get; }
    public Material HatMaterial { get; }
    public Material SelectedHatMaterial { get; }
}
