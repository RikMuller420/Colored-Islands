using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour, ISelectable
{
    private const string OutlineShaderValueName = "_OtlWidth";

    [SerializeField] private MeshRenderer _renderer;

    private float maxOutlineWidth = 10f;
    private float minOutlineWidth = 0f;

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
        _renderer.material.SetFloat(OutlineShaderValueName, maxOutlineWidth);
    }

    public void DeactivateOutline()
    {
        _renderer.material.SetFloat(OutlineShaderValueName, minOutlineWidth);
    }
}
