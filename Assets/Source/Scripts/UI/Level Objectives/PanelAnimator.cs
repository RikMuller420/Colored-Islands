using System.Collections;
using UnityEngine;

public class PanelAnimator : MonoBehaviour
{
    private const string IdleTrigger = "Reset";
    private const string ShakeTrigger = "Shake";
    private const string DropTrigger = "Drop";

    [SerializeField] private Animator _panelAnimator;

    private float _shakeIntertvalMin = 8f;
    private float _shkaeIntervalMax = 15f;
    private Coroutine _movesShakeCorutine;

    public void ResetAnimator()
    {
        StopShaking();
        _panelAnimator.SetTrigger(IdleTrigger);
        StartShaking();
    }

    public void DropPanel()
    {
        StopShaking();
        _panelAnimator.SetTrigger(DropTrigger);
    }

    public void StartShaking()
    {
        StopShaking();
        _movesShakeCorutine = StartCoroutine(Shaking());
    }

    public void StopShaking()
    {
        if (_movesShakeCorutine != null)
        {
            StopCoroutine(_movesShakeCorutine);
        }

        _movesShakeCorutine = null;
    }

    private IEnumerator Shaking()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(_shakeIntertvalMin, _shkaeIntervalMax));

            _panelAnimator.SetTrigger(ShakeTrigger);
        }
    }
}
