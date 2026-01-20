using UnityEngine;

public class UnitCustomizationSettings
{
    public UnitCustomizationSettings(UnitSlotType slot, ColorSample colorSample, Material unitMaterial, Material selectedUnitMaterial,
                                    UnitHatData hatData, Material hatMaterial, Material selectedHatMaterial,
                                    Color trailColor)
    {
        Slot = slot;
        ColorSample = colorSample;
        UnitMaterial = unitMaterial;
        SelectedUnitMaterial = new SelectedUnitMaterial(selectedUnitMaterial);
        HatData = hatData;
        HatMaterial = hatMaterial;
        SelectedHatMaterial = selectedHatMaterial;
        TrailColor = trailColor;
    }

    public UnitSlotType Slot { get; }
    public ColorSample ColorSample { get; }
    public Material UnitMaterial { get; }
    public SelectedUnitMaterial SelectedUnitMaterial { get; }
    public bool IsHatEquiped => HatData != null;
    public UnitHatData HatData { get; }
    public Material HatMaterial { get; }
    public Material SelectedHatMaterial { get; }
    public Color TrailColor { get; }
}
