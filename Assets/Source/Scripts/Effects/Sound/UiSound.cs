using UnityEngine;

namespace SlimeGround.Effects.Sound
{
	[System.Serializable]
	public class UiSound
	{
	    [SerializeField] private UiSoundType _type;
	    [SerializeField] private AudioSource _audioSource;

	    public UiSoundType Type => _type;
	    public AudioSource AudioSource => _audioSource;
	}
}
