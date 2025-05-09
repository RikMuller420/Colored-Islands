using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Unit : PoolableObject, ISelectable
{
    [SerializeField] private MeshRenderer _renderer;

    private Collider _collider;
    private UnitRenderer _unitRenderer;

    public void Initialize(BaseIsland island, Paint paint, PaintMaterials paintMaterials)
    {
        _collider = GetComponent<Collider>();
        Island = island;
        Paint = paint;
        _unitRenderer = new UnitRenderer(_renderer, paintMaterials);
        _unitRenderer.SetPaint(paint);
        Activate();
    }

    public Paint Paint { get; private set; }
    public BaseIsland Island { get; private set; }

    public void ActivateOutline() => _unitRenderer.ActivateOutline();
    public void DeactivateOutline() => _unitRenderer.DeactivateOutline();

    public void SetIsland(BaseIsland island)
    {
        Island = island;
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
}
