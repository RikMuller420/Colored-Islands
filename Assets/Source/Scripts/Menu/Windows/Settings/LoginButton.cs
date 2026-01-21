using SlimeGround.Integration.Authorization;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Settings
{
	public class LoginButton : MonoBehaviour
	{
	    [SerializeField] private Button _button;

	    private AuthorizationProvider _authorizationProvider;

	    private void OnEnable()
	    {
	        _button.onClick.AddListener(AskForLogin);
	        _authorizationProvider.AuthorizationStatusChanged += UpdateButtonActivity;
	    }

	    private void OnDisable()
	    {
	        _button.onClick.RemoveListener(AskForLogin);
	        _authorizationProvider.AuthorizationStatusChanged -= UpdateButtonActivity;
	    }

	    public void Initialize(AuthorizationProvider authorizationProvider)
	    {
	        _authorizationProvider = authorizationProvider;
	        UpdateButtonActivity();
	        enabled = true;
	    }

	    private void AskForLogin()
	    {
	        _authorizationProvider.AskForAuthorization();
	    }

	    private void UpdateButtonActivity()
	    {
	        gameObject.SetActive(_authorizationProvider.IsAuthorized == false);
	    }
	}
}
