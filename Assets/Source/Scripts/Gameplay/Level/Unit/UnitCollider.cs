using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class UnitCollider : MonoBehaviour, ISelectable
{
    [SerializeField] private Unit _unit;

    public Unit Unit => _unit;
}
