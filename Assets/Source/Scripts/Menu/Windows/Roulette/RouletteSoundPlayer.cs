using UnityEngine;

namespace SlimeGround.Menu.Windows.Roulette
{
	public class RouletteSoundPlayer : MonoBehaviour
    {
		private const float AnglePerSound = 30f;
		private const float MinPithch = 0.9f;
		private const float MaxPitch = 1.2f;

		[SerializeField] private AudioSource _tickAudioSource;

		private float _lastPlayedAngle = 0f;

		public void TryPlayTickSound(float angle)
		{
			angle = Mathf.Repeat(angle, 360f);
			float angleDelta = Mathf.Abs(angle - _lastPlayedAngle);

			if (angleDelta > AnglePerSound)
			{
				PlaySound();
				_lastPlayedAngle = angle;
			}
		}

		private void PlaySound()
		{
			_tickAudioSource.pitch = Random.Range(MinPithch, MaxPitch);
			_tickAudioSource.Play();
		}
	}
}
