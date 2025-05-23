using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundVolumeProvider
{
    private AudioMixers _mixers;
    private GameProgressStorage _gameProgressStorage;

    public event Action<AudioGroup> SoundEnabledChanged;

    public SoundVolumeProvider(AudioMixers mixers, GameProgressStorage gameProgressStorage)
    {
        _mixers = mixers;
        _gameProgressStorage = gameProgressStorage;

        gameProgressStorage.SoundEnabledChanged += SoundEnabledChanged;

        foreach (AudioMixerData mixer in _mixers.Mixers)
        {
            UpdateAudioGroupVolume(mixer.AudioGroup);
        }
    }

    public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _gameProgressStorage.GetIsSoundOnStatus(audioGroup);

    public void SetAudioGroupVolume(AudioGroup audioGroup, bool isVolumeOn)
    {
        _gameProgressStorage.SetSoundToggle(audioGroup, isVolumeOn);
        _gameProgressStorage.Save();

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
