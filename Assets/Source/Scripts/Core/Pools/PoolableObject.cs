using UnityEngine;

namespace SlimeGround.Core.Pools
{

	public abstract class PoolableObject : MonoBehaviour
	{
	    public event System.Action<PoolableObject> Deactivated;

	    protected void OnDeactivated()
	    {
	        Deactivated?.Invoke(this);
	    }
	}

}
