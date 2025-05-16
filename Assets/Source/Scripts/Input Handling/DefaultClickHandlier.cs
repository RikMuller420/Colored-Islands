using UnityEngine;

public class DefaultClickHandlier : IClickHandler
{
    private SelectHandler _selectHandler;

    public DefaultClickHandlier(SelectHandler selectHandler)
    {
        _selectHandler = selectHandler;
    }

    public void HandleClick(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out ISelectable selectable))
        {
            _selectHandler.Select(selectable);
        }
    }
}
