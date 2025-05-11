using TMPro;
using UnityEngine;

public class FpsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private float _deltaTime = 0.0f;
    private float _smoothingFactor = 0.1f;


    private void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * _smoothingFactor;
        float fps = 1f / _deltaTime;

        _text.text = $"TS: {Time.timeScale:F2} | FPS: {Mathf.RoundToInt(fps)} ";
    }
}
