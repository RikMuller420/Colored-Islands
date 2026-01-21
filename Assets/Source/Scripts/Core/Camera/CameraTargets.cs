using UnityEngine;

namespace SlimeGround.Core.CameraSystem
{

	[System.Serializable]
	public class CameraTargets
	{
	    [SerializeField] private Transform _lookAtPoint;
	    [SerializeField] private Transform _followPoint;

	    public CameraTargets(Transform lookAtPoint, Transform followPoint)
	    {
	        _lookAtPoint = lookAtPoint;
	        _followPoint = followPoint;
	    }

	    public Transform LookAtPoint => _lookAtPoint;
	    public Transform FollowPoint => _followPoint;
	}

}
