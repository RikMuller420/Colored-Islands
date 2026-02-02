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
	    private AudioMixers _mixers;
	    private PlayerDataProvider _playerData;

	    public SoundVolumeProvider(AudioMixers mixers, PlayerDataProvider playerData)
	    {
	        _mixers = mixers;
	        _playerData = playerData;

	        playerData.SoundEnabledChanged += OnSoundEnabledChanged;

	        foreach (AudioMixerData mixer in _mixers.Mixers)
	        {
	            UpdateAudioGroupVolume(mixer.AudioGroup);
	        }
	    }

		public event Action<AudioGroup> SoundEnabledChanged;

		public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _playerData.GetIsSoundOnStatus(audioGroup);

	    public void SetAudioGroupVolume(AudioGroup audioGroup, bool isVolumeOn)
	    {
	        _playerData.SetSoundToggle(audioGroup, isVolumeOn);
	        _playerData.Save();

	        UpdateAudioGroupVolume(audioGroup);
	    }

	    private void UpdateAudioGroupVolume(AudioGroup audioGroup)
	    {
	        bool isVolumeOn = GetIsSoundOnStatus(audioGroup);
	        float volume = GetDbFromNormalizedValue(isVolumeOn ? 1 : 0);
	        AudioMixerGroup mixer = _mixers.Mixers.FirstOrDefault(mixer => mixer.AudioGroup == audioGroup).Mixer;
	        mixer.audioMixer.SetFloat(audioGroup.ToString(), volume);
	    }

	    private float GetDbFromNormalizedValue(float value)
	    {
	        if (value == 0)
	        {
	            return Constants.MinDbVolume;
	        }

	        return Mathf.Log10(value) * Constants.LogToDbRatio;
	    }

	    private void OnSoundEnabledChanged(AudioGroup audioGroup)
	    {
	        SoundEnabledChanged?.Invoke(audioGroup);
	    }
	}
}
