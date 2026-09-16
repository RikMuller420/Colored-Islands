using System;
using SlimeGround.Effects.Sound;
using SlimeGround.Integration.Localization;

namespace SlimeGround.Data.Saves
{
	public class GameSettingsProvider
	{
		public event Action ShadowActiveStatusChanged;
		public event Action<AudioGroup> SoundEnabledChanged;

		public GameSettingsProvider(PlayerData playerData)
		{
			_playerData = playerData;
		}

		private PlayerData _playerData { get; }

		public bool IsLanguageSaved => _playerData.IsLanguageSaved;
		public Language Language => _playerData.Language;
		public bool IsShadowActiveOnMobile => _playerData.IsShadowActiveOnMobile;
		public bool IsShadowActiveOnDesktop => _playerData.IsShadowActiveOnDesktop;
		public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _playerData.IsSoundOnStatus[audioGroup];

		public void SetLanguage(Language language)
		{
			_playerData.Language = language;
			_playerData.IsLanguageSaved = true;
		}

		public void SetIsShadowActiveOnMobile(bool value)
		{
			_playerData.IsShadowActiveOnMobile = value;
			ShadowActiveStatusChanged?.Invoke();
		}

		public void SetIsShadowActiveOnDesktop(bool value)
		{
			_playerData.IsShadowActiveOnDesktop = value;
			ShadowActiveStatusChanged?.Invoke();
		}

		public void SetSoundToggle(AudioGroup audioGroup, bool isOn)
		{
			_playerData.IsSoundOnStatus[audioGroup] = isOn;
			SoundEnabledChanged?.Invoke(audioGroup);
		}
	}
}
