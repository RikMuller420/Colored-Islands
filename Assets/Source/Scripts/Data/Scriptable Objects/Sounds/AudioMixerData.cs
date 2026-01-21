using SlimeGround.Effects.Sound;
using UnityEngine;
using UnityEngine.Audio;

namespace SlimeGround.Data.ScriptableObjects.Sounds
{
	[System.Serializable]
	public class AudioMixerData 
	{
	    [SerializeField] private AudioGroup _audioGroup;
	    [SerializeField] private AudioMixerGroup _mixer;

	    public AudioGroup AudioGroup => _audioGroup;
	    public AudioMixerGroup Mixer => _mixer;
	}
}
