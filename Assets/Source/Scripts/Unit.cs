using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour, ISelectable
{
    [SerializeField] private MeshRenderer _renderer;

    public void Initialize(Island island, Paint paint)
    {
        Island = island;
        Paint = paint;
        _renderer.material.color = Color.blue;
    }

    public Paint Paint { get; private set; }
    public Island Island { get; private set; }

    public void ActivateOutline()
    {
        //enable outline
    }

    public void DeactivateOutline()
    {
        // disable outline
    }
}
