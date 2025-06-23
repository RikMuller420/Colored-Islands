using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostButtonActivator : MonoBehaviour
{
    [SerializeField] private List<BoostButtonContent> _boostButtons = new();

    public void DeactivateAllButtons()
    {
        foreach (BoostButtonContent button in _boostButtons)
        {
            button.Button.gameObject.SetActive(false);
        }
    }

    public void ActivateAllButtons()
    {
        foreach (BoostButtonContent button in _boostButtons)
        {
            button.Button.gameObject.SetActive(true);
        }
    }
}
