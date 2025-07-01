using System.Collections.Generic;
using UnityEngine;

public class Unit : PoolableObject
{
    [SerializeField] private UnitRenderer _renderer;
    [SerializeField] private Transform _meshTransform;
    [SerializeField] private UnitAnimator _animator;
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform _body;

    private UnitLookAtRotator _lookAtRotator;

    public Paint Paint { get; private set; }
    public BaseIsland Island { get; private set; }
    public Transform MeshTransform => _meshTransform;
    public UnitAnimator Animator => _animator;
    public Collider Collider => _collider;

    public void ActivateOutline() => _renderer.ActivateOutline();
    public void DeactivateOutline() => _renderer.DeactivateOutline();

    public void Initialize(CustomizationSettingsHolder customizationSettings)
    {
        _renderer.Initialize(customizationSettings);
        _lookAtRotator = new UnitLookAtRotator(_body);
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

    public void LookToTarget(Transform target, UnitsMoveInfo unitsMoveInfo) => 
                                            _lookAtRotator.LookToTarget(target, unitsMoveInfo);


#if UNITY_EDITOR
    public void SetMaterial(Material material)
    {
        _renderer.SetMaterial(material);
    }
#endif
}
