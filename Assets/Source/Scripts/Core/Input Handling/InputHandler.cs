using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SlimeGround.Core.InputHandling
{
	public class InputHandler : MonoBehaviour
	{
	    private PlayerInput _playerInput;

	    public event Action<Vector2> Clicked;

	    private void Awake()
	    {
	        _playerInput = new PlayerInput();
	        _playerInput.Player.Click.performed += OnClick;
	    }

	    private void OnEnable()
	    {
	        _playerInput.Enable();
	    }

	    private void OnDisable()
	    {
	        _playerInput.Disable();
	    }

	    private void OnClick(InputAction.CallbackContext context)
	    {
	        Vector2 inputPosition;

	        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
	        {
	            inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
	        }
	        else
	        {
	            inputPosition = Mouse.current.position.ReadValue();
	        }

	        Clicked?.Invoke(inputPosition);
	    }
	}
}
