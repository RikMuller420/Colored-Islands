using System.Collections;
using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	public class UnitAnimator : MonoBehaviour
	{
	    private const string IdleTriggerName = "Idle";
	    private const string JumpTriggerName = "Jump";
	    private const string WalkBoolName = "Walk";

	    [SerializeField] private Animator _animator;
		[SerializeField] private float _minIdleDelay = 3f;
		[SerializeField] private float _maxIdleDelay = 9f;

		private int _walkAnimationHash;
		private float _jumpDuration = 1f;
		private bool _isJumpAviable = true;
		private WaitForSeconds _jumpWait;

	    private void OnEnable()
	    {
	        StartCoroutine(PlayAnimationWithRandomDelay());
	        _walkAnimationHash = Animator.StringToHash(WalkBoolName);
			_jumpWait = new WaitForSeconds(_jumpDuration);
		}

	    public void FreezeAnimation()
	    {
	        _animator.speed = 0;
	    }

	    public void UnfreezeAnimation()
	    {
	        _animator.speed = 1;
	    }

	    public void TryJump()
	    {
			if (_isJumpAviable)
			{
				_animator.SetTrigger(JumpTriggerName);
				_isJumpAviable = false;
				StartCoroutine(EnableJumpInDelay());
			}
		}

	    public void StartWalk()
	    {
	        _animator.SetBool(_walkAnimationHash, true);
	    }

	    public void StopWalk()
	    {
	        _animator.SetBool(_walkAnimationHash, false);
	    }

		private IEnumerator EnableJumpInDelay()
		{
			yield return _jumpWait;

			_isJumpAviable = true;
		}

		private IEnumerator PlayAnimationWithRandomDelay()
	    {
	        bool isFirstAnimation = true;

	        while (enabled)
	        {
	            float randomDelay = Random.Range(_minIdleDelay, _maxIdleDelay);

	            if (isFirstAnimation)
	            {
	                randomDelay -= _minIdleDelay;
	                isFirstAnimation = false;
	            }

	            yield return new WaitForSeconds(randomDelay);

	            _animator.SetTrigger(IdleTriggerName);
	        }
	    }
	}
}
