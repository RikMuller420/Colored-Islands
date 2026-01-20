using System.Collections;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private const string IdleTriggerName = "Idle";
    private const string JumpTriggerName = "Jump";
    private const string WalkBoolName = "Walk";

    [SerializeField] private Animator _animator;

    private int _walkAnimationHash;
    private float _minIdleDelay = 3f;
    private float _maxIdleDelay = 9f;

    private void OnEnable()
    {
        StartCoroutine(PlayAnimationWithRandomDelay());
        _walkAnimationHash = Animator.StringToHash(WalkBoolName);
    }

    public void FreezeAnimation()
    {
        _animator.speed = 0;
    }

    public void UnfreezeAnimation()
    {
        _animator.speed = 1;
    }

    public void Jump()
    {
        _animator.SetTrigger(JumpTriggerName);
    }

    public void StartWalk()
    {
        _animator.SetBool(_walkAnimationHash, true);
    }

    public void StopWalk()
    {
        _animator.SetBool(_walkAnimationHash, false);
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
