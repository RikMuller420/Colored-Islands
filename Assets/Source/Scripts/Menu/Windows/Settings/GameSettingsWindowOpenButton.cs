using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Settings
{

	public class GameSettingsWindowOpenButton : MonoBehaviour
	{
	    [SerializeField] private Button _button;
	    [SerializeField] private InGameSettingsWindow _window;

	    private void OnEnable()
	    {
	        _button.onClick.AddListener(Open);
	    }

	    private void OnDisable()
	    {
	        _button.onClick.RemoveListener(Open);
	    }

	    protected virtual void Open()
	    {
	        _window.Open();
	    }
	}

}
