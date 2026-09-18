using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Effects.Sound
{
	public class UISoundPlayer : MonoBehaviour
	{
		private const float MinPithch = 0.9f;
		private const float MaxPitch = 1.1f;

		[SerializeField] private List<UiSound> _sounds;

	    public void PlaySound(UiSoundType type)
	    {
	        AudioSource audioSource = _sounds.Find(sound => sound.Type == type).AudioSource;
	        audioSource.pitch = Random.Range(MinPithch, MaxPitch);
	        audioSource.Play();
	    }
	}
}