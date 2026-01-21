using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Effects.Sound
{

	public class UISoundPlayer : MonoBehaviour
	{
	    [SerializeField] private List<UiSound> _sounds;

	    private float _minPithch = 0.9f;
	    private float _maxPitch = 1.1f;

	    public void PlaySound(UiSoundType type)
	    {
	        AudioSource audioSource = _sounds.Find(sound => sound.Type == type).AudioSource;
	        audioSource.pitch = Random.Range(_minPithch, _maxPitch);
	        audioSource.Play();
	    }
	}

}
