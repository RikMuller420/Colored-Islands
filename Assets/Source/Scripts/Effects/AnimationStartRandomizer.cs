using UnityEngine;

namespace SlimeGround.Effects
{
	public class AnimationStartRandomizer : MonoBehaviour
    {
		[SerializeField] private Animator _animator;
		[SerializeField] private string _animationName = "Idle";
		[SerializeField] private int _layerIndex = 0;

		private void Start()
		{
			if (_animator != null)
			{
				float randomNormalizedTime = Random.Range(0f, 1f);
				_animator.Play(_animationName, _layerIndex, randomNormalizedTime);
				_animator.Update(0f);
			}

		}
	}
}
