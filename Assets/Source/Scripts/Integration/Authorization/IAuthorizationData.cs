using System;

namespace SlimeGround.Integration.Authorization
{
	public interface IAuthorizationData
	{
	    public event Action AuthorizationStatusChanged;

	    public bool IsAuthorized { get; }
	}
}
