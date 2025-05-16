using UnityEngine;

public class ClickHandlerData
{
    public IClickHandler ClickHandler { get; }
    public LayerMask LayerMask { get; }
    public float MaxClickDistance { get; }

    public ClickHandlerData(IClickHandler clickHandler, LayerMask layerMask,
                            float maxClickDistance = 1000f)
    {
        ClickHandler = clickHandler;
        LayerMask = layerMask;
        MaxClickDistance = maxClickDistance;
    }
}
