using UnityEngine;

public class TabContent : MonoBehaviour
{
    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivte()
    {
        gameObject.SetActive(false);
    }
}
