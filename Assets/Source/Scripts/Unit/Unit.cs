using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Unit : PoolableObject, ISelectable
{
    private const string IdleTriggerName = "Idle";

    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Transform _meshTransform;
    [SerializeField] private Animator _animator;

    private Collider _collider;
    private UnitRenderer _unitRenderer;

    private float _minIdleDelay = 3f;
    private float _maxIdleDelay = 9f;

    private void OnEnable()
    {
        StartCoroutine(PlayAnimationWithRandomDelay());
    }

    public void Initialize(BaseIsland island, Paint paint, PaintMaterials paintMaterials)
    {
        _collider = GetComponent<Collider>();
        Island = island;
        SetPaint(paint, paintMaterials);
        Activate();
    }

    public Paint Paint { get; private set; }
    public BaseIsland Island { get; private set; }
    public Transform MeshTransform => _meshTransform;

    public void ActivateOutline() => _unitRenderer.ActivateOutline();
    public void DeactivateOutline() => _unitRenderer.DeactivateOutline();

    public void SetIsland(BaseIsland island)
    {
        Island = island;
    }

    public void SetPaint(Paint paint, PaintMaterials paintMaterials)
    {
        Paint = paint;
        _unitRenderer = new UnitRenderer(_renderer, paintMaterials);
        _unitRenderer.SetPaint(paint);
    }

    public void Deactivate()
    {
        enabled = false;
        _collider.enabled = false;
    }

    private void Activate()
    {
        enabled = true;
        _collider.enabled = true;
    }

    private IEnumerator PlayAnimationWithRandomDelay()
    {
        bool isFirstAnimation = true;

        while (enabled)
        {
            float randomDelay = Random.Range(_minIdleDelay, _maxIdleDelay);

            if (isFirstAnimation)
            {
                randomDelay -= _minIdleDelay;
                isFirstAnimation = false;
            }

            yield return new WaitForSeconds(randomDelay);

            _animator.SetTrigger(IdleTriggerName);
        }
    }
}
