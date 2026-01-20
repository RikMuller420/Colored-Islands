using System;

public interface IAuthorizationData
{
    public event Action AuthorizationStatusChanged;

    public bool IsAuthorized { get; }
}
