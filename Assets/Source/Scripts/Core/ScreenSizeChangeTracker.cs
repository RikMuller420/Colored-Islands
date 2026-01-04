using System;
using System.Collections;
using UnityEngine;

public class ScreenSizeChangeTracker : MonoBehaviour
{
    private Vector2 _lastScreenSize = Vector2.zero;
    private float _resizeCheckInterval = 0.1f;
    private WaitForSeconds _wait;

    public event Action<Vector2> ScreenSizeChanged;

    private void Start()
    {
        _wait = new WaitForSeconds(_resizeCheckInterval);
        StartCoroutine(CheckScreenSize());
    }
    
    private IEnumerator CheckScreenSize()
    {
        while (enabled)
        {
            Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);

            if (currentScreenSize != _lastScreenSize)
            {
                ScreenSizeChanged?.Invoke(currentScreenSize);
                _lastScreenSize = currentScreenSize;
            }

            yield return _wait;
        }
    }
}
