using UnityEngine;

namespace SlimeGround.Effects.Sound
{
	[System.Serializable]

	public class GameplaySound
	{
	    [SerializeField] private GameplaySoundType _type;
	    [SerializeField] private AudioSource _audioSource;

	    public GameplaySoundType Type => _type;
	    public AudioSource AudioSource => _audioSource;
	}
}
