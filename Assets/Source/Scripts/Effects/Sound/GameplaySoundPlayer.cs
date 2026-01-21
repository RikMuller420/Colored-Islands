using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Effects.Sound
{

	public class GameplaySoundPlayer : MonoBehaviour
	{
	    [SerializeField] private List<GameplaySound> _sounds;

	    public void PlaySound(GameplaySoundType type)
	    {
	        AudioSource audioSource = _sounds.Find(sound => sound.Type == type).AudioSource;
	        audioSource.Play();
	    }
	}

}
