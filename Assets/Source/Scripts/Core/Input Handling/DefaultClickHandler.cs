using UnityEngine;

public class DefaultClickHandler : ClickBehaviour
{
    private SelectHandler _selectHandler;

    public DefaultClickHandler(SelectHandler selectHandler, LayerMask layerMask) : base(layerMask)
    {
        _selectHandler = selectHandler;
    }

    public override void HandleClick(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out ISelectable selectable))
        {
            _selectHandler.Select(selectable);
        }
    }
}
