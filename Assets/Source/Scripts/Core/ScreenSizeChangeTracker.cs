using System;
using System.Collections;
using UnityEngine;

namespace SlimeGround.Core
{
	public class ScreenSizeChangeTracker : MonoBehaviour
	{
	    private const  float RefreshRate = 0.1f;

	    private Vector2 _lastScreenSize = Vector2.zero;
	    private WaitForSeconds _wait;

	    public event Action<Vector2> ScreenSizeChanged;

	    private void Start()
	    {
	        _wait = new WaitForSeconds(RefreshRate);
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
}
