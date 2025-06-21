using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Unit : PoolableObject, ISelectable
{
    [SerializeField] private UnitRenderer _renderer;
    [SerializeField] private Transform _meshTransform;
    [SerializeField] private UnitAnimator _animator;
    [SerializeField] private Collider _collider;


    public Paint Paint { get; private set; }
    public BaseIsland Island { get; private set; }
    public Transform MeshTransform => _meshTransform;
    public UnitAnimator Animator => _animator;

    public void ActivateOutline() => _renderer.ActivateOutline();
    public void DeactivateOutline() => _renderer.DeactivateOutline();

    public void Initialize(CustomizationSettingsHolder customizationSettings)
    {
        _renderer.Initialize(customizationSettings);
    }

    public void SetIsland(BaseIsland island)
    {
        Island = island;
    }

    public void SetPaint(Paint paint)
    {
        Paint = paint;
        _renderer.SetPaint(paint);
    }

    public void Deactivate()
    {
        enabled = false;
        _collider.enabled = false;
    }

    public void Activate()
    {
        enabled = true;
        _collider.enabled = true;
    }
}
