using System;
using System.Collections.Generic;
using SlimeGround.Core;
using SlimeGround.Menu.Boosts;
using UnityEngine;

namespace SlimeGround.Menu.OrientationChanger
{
	public class UIOrientationChanger : MonoBehaviour
	{
	    [SerializeField] private ScreenSizeChangeTracker _screenSizeChangeTracker;
	    [SerializeField] private GameObject _boostsZoneVertical;
	    [SerializeField] private GameObject _boostsZoneHorizontal;
	    [SerializeField] private List<BoostButton> _boostButtons;

	    public event Action OrientationChanged;

	    public bool IsVertical { get; private set; } = true;

	    private void OnEnable()
	    {
	        _screenSizeChangeTracker.ScreenSizeChanged += TryUpdateOrientation;
	    }

	    private void OnDisable()
	    {
	        _screenSizeChangeTracker.ScreenSizeChanged -= TryUpdateOrientation;
	    }

	    private void TryUpdateOrientation(Vector2 screenSize)
	    {
	        bool isNewOrientationVertical = screenSize.y > screenSize.x;

	        if (isNewOrientationVertical != IsVertical)
	        {
	            IsVertical = isNewOrientationVertical;
	            UpdateOrientation();
	            OrientationChanged?.Invoke();
	        }
	    }

	    private void UpdateOrientation()
	    {
	        _boostsZoneVertical.SetActive(IsVertical);
	        _boostsZoneHorizontal.SetActive(IsVertical == false);

	        Transform boostButtonParent = IsVertical ? _boostsZoneVertical.transform : 
	                                                    _boostsZoneHorizontal.transform;

	        foreach (BoostButton button in _boostButtons)
	        {
	            button.transform.SetParent(boostButtonParent);
	        }
	    }
	}
}
