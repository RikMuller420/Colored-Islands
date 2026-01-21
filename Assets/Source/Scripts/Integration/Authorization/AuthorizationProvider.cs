using System;
using YG;

namespace SlimeGround.Integration.Authorization
{

	public class AuthorizationProvider : IAuthorizationData
	{
	    public event Action AuthorizationStatusChanged;

	    public AuthorizationProvider()
	    {
	        YG2.onGetSDKData += OnAuthorizationStatusChanged;
	    }

	    public bool IsAuthorized => YG2.player.auth;

	    public void AskForAuthorization()
	    {
	        YG2.OpenAuthDialog();
	    }

	    private void OnAuthorizationStatusChanged()
	    {
	        AuthorizationStatusChanged?.Invoke();
	    }
	}

}
