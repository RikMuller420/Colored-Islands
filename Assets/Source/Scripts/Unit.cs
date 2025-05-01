using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour, ISelectable
{
    [SerializeField] private MeshRenderer _renderer;

    private UnitRenderer _unitRenderer;

    public void Initialize(Island island, Paint paint)
    {
        Island = island;
        Paint = paint;
        _unitRenderer = new UnitRenderer(_renderer);

        switch (Paint)
        {
            case Paint.Red:
                _renderer.material.color = Color.red;
                break;

            case Paint.Blue:
                _renderer.material.color = Color.blue;
                break;

            case Paint.Green:
                _renderer.material.color = Color.green;
                break;

            case Paint.Yellow:
                _renderer.material.color = Color.yellow;
                break;

            case Paint.Pink:
                _renderer.material.color = Color.magenta;
                break;

        }
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
