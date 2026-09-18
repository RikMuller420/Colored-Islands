using System;
using System.Linq;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Sounds;
using UnityEngine;
using UnityEngine.Audio;

namespace SlimeGround.Effects.Sound
{
	public class SoundVolumeProvider
	{
		private const float LogToDbRatio = 20;
		private const float MinDbVolume = -80f;
		private const float MaxVolume = 0.3f;

		private readonly AudioMixers _mixers;
		private readonly PlayerDataProvider _playerData;

		public SoundVolumeProvider(AudioMixers mixers, PlayerDataProvider playerData)
	    {
	        _mixers = mixers;
	        _playerData = playerData;

			_playerData.Settings.SoundEnabledChanged += OnSoundEnabledChanged;

	        foreach (AudioMixerData mixer in _mixers.Mixers)
	        {
	            UpdateAudioGroupVolume(mixer.AudioGroup);
	        }
	    }

		public event Action<AudioGroup> SoundEnabledChanged;

		public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _playerData.Settings.GetIsSoundOnStatus(audioGroup);

		public void Dispose()
		{
			_playerData.Settings.SoundEnabledChanged -= OnSoundEnabledChanged;
		}

		public void SetAudioGroupVolume(AudioGroup audioGroup, bool isVolumeOn)
	    {
	        _playerData.Settings.SetSoundToggle(audioGroup, isVolumeOn);
	        _playerData.Save();

	        UpdateAudioGroupVolume(audioGroup);
	    }

	    private void UpdateAudioGroupVolume(AudioGroup audioGroup)
	    {
	        bool isVolumeOn = GetIsSoundOnStatus(audioGroup);
	        float volume = GetDbFromNormalizedValue(isVolumeOn ? MaxVolume : 0);
	        AudioMixerGroup mixer = _mixers.Mixers.FirstOrDefault(mixer => mixer.AudioGroup == audioGroup).Mixer;
	        mixer.audioMixer.SetFloat(audioGroup.ToString(), volume);
	    }

	    private float GetDbFromNormalizedValue(float value)
	    {
	        if (value == 0)
	        {
	            return MinDbVolume;
	        }

	        return Mathf.Log10(value) * LogToDbRatio;
	    }

	    private void OnSoundEnabledChanged(AudioGroup audioGroup)
	    {
	        SoundEnabledChanged?.Invoke(audioGroup);
	    }
	}
}
