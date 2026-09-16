using System;
using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Data.ScriptableObjects.Paints;
using SlimeGround.Data.ScriptableObjects.UnitFaces;
using SlimeGround.Gameplay.Units;
using SlimeGround.Integration.DeviceInfo;
using UnityEngine;
using UnityEngine.Rendering;

namespace SlimeGround.Menu.Windows.Customization
{
	public class CustomizationSettingsHolder
	{
	    private const string UnitFaceTextureName = "_OverlayTex";

		public ShadowCastingMode ShadowCastingMode { get; private set; }

	    public CustomizationSettingsHolder(ColorSampleMaterials paintMaterials, IPlayerData playerData,
	                                       UnitsFaceSettings faceSettings, UnitsHatSettings hatSettings)
	    {
	        _paintMaterials = paintMaterials;
	        _playerData = playerData;
	        _faceSettings = faceSettings;
	        _hatSettings = hatSettings;

			SetShadowCastingMode();

			foreach (UnitSlotType slot in Enum.GetValues(typeof(UnitSlotType)))
	        {
	            _customizationSettings.Add(CreateSettings(slot));
	        }

	        _playerData.Customization.CustomizationPreferenceChanged += OnCustomizationPreferenceChanged;
			_playerData.Settings.ShadowActiveStatusChanged += SetShadowCastingMode;
		}

		private ColorSampleMaterials _paintMaterials { get; }
		private IPlayerData _playerData { get; }
		private UnitsFaceSettings _faceSettings { get; }
		private UnitsHatSettings _hatSettings { get; }
		private DeviceInfoProvider _deviceInfoProvider { get; } = new();
		private List<UnitCustomizationSettings> _customizationSettings { get; } = new();

		public void Dispose()
		{
			_playerData.Customization.CustomizationPreferenceChanged -= OnCustomizationPreferenceChanged;
		}

		public UnitCustomizationSettings GetCustomizationSettings(UnitSlotType slot)
		{
			return _customizationSettings.FirstOrDefault(settings => settings.Slot == slot);
		}

		private void SetShadowCastingMode()
		{
			Integration.DeviceInfo.DeviceType deviceType = _deviceInfoProvider.GetDeviceType();

			bool isShadowActive = deviceType == Integration.DeviceInfo.DeviceType.Mobile ?
									_playerData.Settings.IsShadowActiveOnMobile :
									_playerData.Settings.IsShadowActiveOnDesktop;

			ShadowCastingMode = isShadowActive ? ShadowCastingMode.On : ShadowCastingMode.Off;
		}

	    private void OnCustomizationPreferenceChanged(UnitSlotType slot)
	    {
	        int settingsIndex = _customizationSettings.FindIndex(settings => settings.Slot == slot);
	        _customizationSettings[settingsIndex] = CreateSettings(slot);
	    }

	    private UnitCustomizationSettings CreateSettings(UnitSlotType slot)
	    {
	        CustomizationPreferences preference = _playerData.Customization.GetCustomizationPreference(slot);
	        int faceId = preference.FaceId;
	        int hatId = preference.HatId;
	        ColorSample colorSample = preference.ColorSample;

	        ColorSampleMaterialData materialData = _paintMaterials.Materials.FirstOrDefault(material => material.ColorSample == colorSample);
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
				CreateHatMaterials(materialData.HatMaterial),
				CreateHatMaterials(materialData.SelectedHatMaterial),
	            materialData.UnitUiColor
	        );
	    }

		private Dictionary<HatTextureType, Material> CreateHatMaterials(Material referenceMaterial)
		{
			Dictionary<HatTextureType, Material> hatMaterials = new ();

			foreach (HatTextureType textureType in Enum.GetValues(typeof(HatTextureType)))
			{
				Material material = new Material(referenceMaterial);
				material.mainTexture = _hatSettings.HatTextures.First(hatTexture => hatTexture.Type == textureType).Texture;
				hatMaterials.Add(textureType, material);
			}

			return hatMaterials;
		}

	    private void PrepareUnitMaterial(Material material, UnitFaceData faceData)
	    {
	        material.SetTexture(UnitFaceTextureName, faceData.Texture);
	        material.SetTextureScale(UnitFaceTextureName, faceData.Tilling);
	        material.SetTextureOffset(UnitFaceTextureName, faceData.Offset);
	    }
	}
}
