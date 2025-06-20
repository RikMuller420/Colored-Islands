using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _lockIcon;
    [SerializeField] private GameObject _selectedFrame;

    protected Button Button => _button;

    public void SetLockedStyle()
    {
        _button.interactable = false;
        _lockIcon.SetActive(true);
    }

    public void SetUnlockedStyle()
    {
        _button.interactable = true;
        _lockIcon.SetActive(false);
    }

    public void SetSelectedStyle()
    {
        _button.interactable = false;
        _selectedFrame.SetActive(true);
    }

    public void SetNonSelectedStyle()
    {
        _button.interactable = true;
        _selectedFrame.SetActive(false);
    }
}
