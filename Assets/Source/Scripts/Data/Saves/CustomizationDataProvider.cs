using System;
using System.Collections.Generic;
using SlimeGround.Menu.Windows.Customization;

namespace SlimeGround.Data.Saves
{
	public class CustomizationDataProvider
	{
		private PlayerData _playerData;

		public event Action<UnitSlotType> CustomizationPreferenceChanged;
		public event Action<int> FaceUnlocked;

		public CustomizationDataProvider(PlayerData playerData)
		{
			_playerData = playerData;
		}

		public IReadOnlyCollection<FaceAvailabilitie> FaceAvailabilities => _playerData.FaceAvailabilities;
		public CustomizationPreferences GetCustomizationPreference(UnitSlotType unitSlot) => _playerData.CustomizationPreferences[unitSlot];
		public void MarkHatUsed(int hatId) => _playerData.IsHatsUsed[hatId] = true;

		public bool IsHatUsed(int hatId)
		{
			return _playerData.IsHatsUsed.ContainsKey(hatId) ?
										_playerData.IsHatsUsed[hatId]
										: true;
		}

		public void UnlockFace(int faceId)
		{
			FaceAvailabilitie face = _playerData.FaceAvailabilities.Find(face => face.FaceId == faceId);
			FaceAvailabilitie newFace = new FaceAvailabilitie(face.FaceId, true, face.WasUsed);

			_playerData.FaceAvailabilities.Remove(face);
			_playerData.FaceAvailabilities.Add(newFace);

			FaceUnlocked?.Invoke(faceId);
		}

		public void MarkFaceUsed(int faceId)
		{
			FaceAvailabilitie face = _playerData.FaceAvailabilities.Find(face => face.FaceId == faceId);
			FaceAvailabilitie newFace = new FaceAvailabilitie(face.FaceId, face.IsAviable, true);

			_playerData.FaceAvailabilities.Remove(face);
			_playerData.FaceAvailabilities.Add(newFace);
		}

		public void ChangeCustomizationPreferenceFace(UnitSlotType unitSlot, int faceId)
		{
			CustomizationPreferences preference = _playerData.CustomizationPreferences[unitSlot];
			int hatId = preference.HatId;
			_playerData.CustomizationPreferences[unitSlot] = new CustomizationPreferences(faceId, hatId, preference.ColorSample);

			CustomizationPreferenceChanged?.Invoke(unitSlot);
		}

		public void ChangeCustomizationPreferenceHat(UnitSlotType unitSlot, int hatId)
		{
			CustomizationPreferences preference = _playerData.CustomizationPreferences[unitSlot];
			int faceId = preference.FaceId;
			_playerData.CustomizationPreferences[unitSlot] = new CustomizationPreferences(faceId, hatId, preference.ColorSample);

			CustomizationPreferenceChanged?.Invoke(unitSlot);
		}

		public void ChangeCustomizationPreferenceColor(UnitSlotType unitSlot, ColorSample colorSample)
		{
			CustomizationPreferences preference = _playerData.CustomizationPreferences[unitSlot];
			int hatId = preference.HatId;
			int faceId = preference.FaceId;
			_playerData.CustomizationPreferences[unitSlot] = new CustomizationPreferences(faceId, hatId, colorSample);

			CustomizationPreferenceChanged?.Invoke(unitSlot);
		}
	}
}
