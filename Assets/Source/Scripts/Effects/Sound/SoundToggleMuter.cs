using SlimeGround.Menu.Extensions.Controls;
using UnityEngine;

namespace SlimeGround.Effects.Sound
{
	public class SoundToggleMuter : MonoBehaviour
	{
	    [SerializeField] private ToggleButton _toggle;
	    [SerializeField] private AudioGroup _audioGroup;

	    private SoundVolumeProvider _soundVolumeProvider;

	    private void OnEnable()
	    {
	        _toggle.ValueChanged += OnToggleChanged;
	        _soundVolumeProvider.SoundEnabledChanged += OnSoundEnabledChanged;
	    }

	    private void OnDisable()
	    {
	        _toggle.ValueChanged -= OnToggleChanged;
	        _soundVolumeProvider.SoundEnabledChanged -= OnSoundEnabledChanged;
	    }

	    public void Initialize(SoundVolumeProvider soundVolumeProvider)
	    {
	        _soundVolumeProvider = soundVolumeProvider;
	        enabled = true;
	        OnSoundEnabledChanged(_audioGroup);
	    }

	    private void OnSoundEnabledChanged(AudioGroup audioGroup)
	    {
	        if (_audioGroup == audioGroup)
	        {
	            bool isOn = _soundVolumeProvider.GetIsSoundOnStatus(_audioGroup);
	            _toggle.SetToggle(isOn);
	        }
	    }

	    private void OnToggleChanged(bool isOn)
	    {
	        _soundVolumeProvider.SetAudioGroupVolume(_audioGroup, isOn);
	    }
	}
}
