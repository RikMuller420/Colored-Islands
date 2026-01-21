using UnityEngine;

namespace SlimeGround.Core.InputHandling
{

	public abstract class ClickBehaviour
	{
	    public LayerMask LayerMask { get; }
	    public float MaxClickDistance { get; }

	    public ClickBehaviour(LayerMask layerMask, float maxClickDistance = 1000f)
	    {
	        LayerMask = layerMask;
	        MaxClickDistance = maxClickDistance;
	    }

	    public abstract void HandleClick(RaycastHit hit);
	    public abstract void ResetBehaviour();
	}

}
