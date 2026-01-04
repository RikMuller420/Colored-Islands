using UnityEngine;

public class TabContent : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public virtual void Activate()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Deactivte()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }
}
