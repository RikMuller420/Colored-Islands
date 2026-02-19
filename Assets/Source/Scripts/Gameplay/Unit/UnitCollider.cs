using SlimeGround.Core.InputHandling;
using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	[RequireComponent(typeof(CapsuleCollider))]
	public class UnitCollider : MonoBehaviour, ISelectable
	{
		[SerializeField] private CapsuleCollider _collider;
		[SerializeField] private Unit _unit;

		public Unit Unit => _unit;

		public void Activate()
		{
			_collider.enabled = true;
		}

		public void Deactivate()
		{
			_collider.enabled = false;
		}
	}
}
