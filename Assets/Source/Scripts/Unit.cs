using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour, ISelectable
{
    [SerializeField] private MeshRenderer _renderer;

    private UnitRenderer _unitRenderer;

    public void Initialize(Island island, Paint paint, PaintMaterials paintMaterials)
    {
        Island = island;
        Paint = paint;
        _unitRenderer = new UnitRenderer(_renderer, paintMaterials);
        _unitRenderer.SetPaint(paint);
    }

    public Paint Paint { get; private set; }
    public Island Island { get; private set; }

    public void ActivateOutline() => _unitRenderer.ActivateOutline();
    public void DeactivateOutline() => _unitRenderer.DeactivateOutline();


    public void SetIsland(Island island)
    {
        Island = island;
    }
}
