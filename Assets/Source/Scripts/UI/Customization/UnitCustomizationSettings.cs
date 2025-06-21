using UnityEngine;

public class UnitCustomizationSettings
{
    public UnitCustomizationSettings(Paint paint, Material[] unitMaterials, UnitHatData hatData, Material hatMaterial)
    {
        Paint = paint;
        UnitMaterials = unitMaterials;
        HatData = hatData;
        HatMaterial = hatMaterial;
    }

    public Paint Paint { get; }
    public Material[] UnitMaterials { get; }
    public bool IsHatEquiped => HatData != null;
    public UnitHatData HatData { get; }
    public Material HatMaterial { get; }
}
