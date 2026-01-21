using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Extensions.Windows
{

	public class MenuWindowOpener : MonoBehaviour
	{
	    [SerializeField] private Button _button;
	    [SerializeField] private MenuWindow _window;

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
