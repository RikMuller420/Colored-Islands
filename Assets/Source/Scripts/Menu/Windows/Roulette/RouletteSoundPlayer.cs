using UnityEngine;

namespace SlimeGround.Menu.Windows.Roulette
{
	public class RouletteSoundPlayer : MonoBehaviour
    {
		[SerializeField] private AudioSource _tickAudioSource;

		private float _lastPlayedAngle = 0f;
		private float _anglePerSound = 30f;
		private float _minPithch = 0.9f;
		private float _maxPitch = 1.2f;

		public void TryPlayTickSound(float angle)
		{
			angle = Mathf.Repeat(angle, 360f);
			float angleDelta = Mathf.Abs(angle - _lastPlayedAngle);

			if (angleDelta > _anglePerSound)
			{
				PlaySound();
				_lastPlayedAngle = angle;
			}
		}

		private void PlaySound()
		{
			_tickAudioSource.pitch = Random.Range(_minPithch, _maxPitch);
			_tickAudioSource.Play();
		}
	}
}
