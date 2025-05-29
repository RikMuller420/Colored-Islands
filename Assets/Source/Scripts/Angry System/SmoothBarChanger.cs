using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothBarChanger : MonoBehaviour
{
    [SerializeField] private AngryBar _angryBar;
    [SerializeField] private Image _filler;

    private float changeDuration = 0.3f;
    private float _startValue;
    private float _targetValue;
    private float _time;
    private Coroutine _changeBarCoroutine;
    private WaitForEndOfFrame _wait = new WaitForEndOfFrame();

    private void Awake()
    {
        UpdateFiller();
    }

    private void OnEnable()
    {
        _angryBar.Changed += UpdateFiller;
    }

    private void OnDisable()
    {
        _angryBar.Changed -= UpdateFiller;
    }

    private void UpdateFiller()
    {
        if (_changeBarCoroutine != null)
        {
            StopCoroutine(_changeBarCoroutine);
        }

        _changeBarCoroutine = StartCoroutine(ChangingBarValue());
    }

    private IEnumerator ChangingBarValue()
    {
        _time = 0f;
        _startValue = _filler.fillAmount;
        _targetValue = _angryBar.Value;

        while (_time < changeDuration)
        {
            _time += Time.deltaTime;
            _filler.fillAmount = Mathf.Lerp(_startValue, _targetValue, _time / changeDuration);

            yield return _wait;
        }
    }
}
