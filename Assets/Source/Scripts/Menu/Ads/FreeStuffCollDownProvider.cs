using System;
using System.Collections;
using Lean.Localization;
using UnityEngine;

namespace SlimeGround.Menu.Ads
{
	public class FreeStuffCollDownProvider : MonoBehaviour
	{
		private const int CoolDownSeconds = 150;
		private const int SecondsInMinute = 60;

		[SerializeField] private LeanToken _timerToken;

	    private int _restSeconds = 0;
	    private WaitForSeconds _awaitSecond;
	    private Coroutine _timerCoroutine;

	    public event Action CoolDownStarted;
	    public event Action CoolDownFinished;

	    private void Start()
	    {
	        _awaitSecond = new WaitForSeconds(1);
	    }

		public bool IsAddAviable => _restSeconds <= 0;

		public bool TryUseAdd()
	    {
	        if (_restSeconds > 0)
	        {
	            return false;
	        }

	        StartCoolDown();

	        return true;
	    }

	    private void StartCoolDown()
	    {
	        _restSeconds = CoolDownSeconds;
	        UpdateTimerToken(_restSeconds);

	        CoolDownStarted?.Invoke();

	        if (_timerCoroutine != null)
	        {
	            StopCoroutine(_timerCoroutine);
	        }

	        _timerCoroutine = StartCoroutine(TimerTicking());
	    }

	    private IEnumerator TimerTicking()
	    {
	        while (_restSeconds > 0)
	        {
	            yield return _awaitSecond;

	            _restSeconds--;
	            UpdateTimerToken(_restSeconds);
	        }

	        CoolDownFinished?.Invoke();
	    }

	    private void UpdateTimerToken(int seconds)
	    {
	        int timerMinutes = seconds / SecondsInMinute;
	        int timerSeconds = seconds % SecondsInMinute;
	        string timerText = $"{timerMinutes}:{timerSeconds.ToString("D2")}";

	        _timerToken.SetValue(timerText);
	    }
	}
}
