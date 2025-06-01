using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothBarChanger : MonoBehaviour
{
    [SerializeField] private Image _filler;

    private float changeDuration = 0.3f;
    private float _startValue;
    private float _time;
    private Coroutine _changeBarCoroutine;
    private WaitForEndOfFrame _wait = new WaitForEndOfFrame();

    public void UpdateBarValue(float value)
    {
        if (_changeBarCoroutine != null)
        {
            StopCoroutine(_changeBarCoroutine);
        }

        _changeBarCoroutine = StartCoroutine(ChangingBarValue(value));
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
}
