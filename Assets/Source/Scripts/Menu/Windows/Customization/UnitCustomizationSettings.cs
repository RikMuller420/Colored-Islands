using System.Collections.Generic;
using SlimeGround.Data;
using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Effects;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Customization
{
	public class UnitCustomizationSettings
	{
	    public UnitCustomizationSettings(UnitSlotType slot, ColorSample colorSample, Material unitMaterial,
										 Material selectedUnitMaterial, UnitHatData hatData,
										 Dictionary<HatTextureType, Material> hatMaterials,
										 Dictionary<HatTextureType, Material> selectedHatMaterials,
	                                     Color trailColor)
	    {
	        Slot = slot;
	        ColorSample = colorSample;
	        UnitMaterial = unitMaterial;
	        SelectedUnitMaterial = new SelectedUnitMaterial(selectedUnitMaterial);
	        HatData = hatData;
	        HatMaterials = hatMaterials;
	        SelectedHatMaterials = selectedHatMaterials;
	        TrailColor = trailColor;
	    }

	    public UnitSlotType Slot { get; }
	    public ColorSample ColorSample { get; }
	    public Material UnitMaterial { get; }
	    public SelectedUnitMaterial SelectedUnitMaterial { get; }
	    public bool IsHatEquiped => HatData != null;
	    public UnitHatData HatData { get; }
		public Dictionary<HatTextureType, Material> HatMaterials { get; }
		public Dictionary<HatTextureType, Material> SelectedHatMaterials { get; }
	    public Color TrailColor { get; }
	}
}
