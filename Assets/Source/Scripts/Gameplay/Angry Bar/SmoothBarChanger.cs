using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Gameplay.AngryBar
{
	public class SmoothBarChanger : MonoBehaviour
	{
	    [SerializeField] private Image _filler;
	    [SerializeField] private Animator _animator;

	    private float _slowDownDuration = 2f;
	    private float changeDuration = 0.3f;
	    private float _startValue;
	    private float _time;
	    private Coroutine _changeBarCoroutine;
	    private Coroutine _slowDownCoroutine;
	    private WaitForEndOfFrame _wait = new WaitForEndOfFrame();

	    public void UpdateBarValue(float value)
	    {
	        if (_changeBarCoroutine != null)
	        {
	            StopCoroutine(_changeBarCoroutine);
	        }

	        _changeBarCoroutine = StartCoroutine(ChangingBarValue(value));
	    }

	    public void StartAnimation()
	    {
	        StopSlowDownAnimation();
	        _animator.speed = 1f;
	    }

	    public void StopAnimation()
	    {
	        StopSlowDownAnimation();
	        _slowDownCoroutine = StartCoroutine(SlowDownAnimation());
	    }

	    private void StopSlowDownAnimation()
	    {
	        if (_slowDownCoroutine != null)
	        {
	            StopCoroutine(_slowDownCoroutine);
	        }

	        _slowDownCoroutine = null;
	    }

	    private IEnumerator ChangingBarValue(float targetValue)
	    {
	        _time = 0f;
	        _startValue = _filler.fillAmount;

	        while (_time < changeDuration)
	        {
	            _time += Time.deltaTime;
	            _filler.fillAmount = Mathf.Lerp(_startValue, targetValue, _time / changeDuration);

	            yield return _wait;
	        }
	    }

	    private IEnumerator SlowDownAnimation()
	    {
	        float startSpeed = _animator.speed;
	        float elapsedTime = 0f;

	        while (elapsedTime < _slowDownDuration)
	        {
	            elapsedTime += Time.deltaTime;
	            float time = elapsedTime / _slowDownDuration;
	            _animator.speed = Mathf.Lerp(startSpeed, 0f, time);

	            yield return _wait;
	        }
	    }
	}
}
