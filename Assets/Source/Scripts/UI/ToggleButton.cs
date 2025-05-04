using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private bool _startEnabled = true;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _toggleOn;
    [SerializeField] private GameObject _toggleOff;

    public event Action<bool> EnableChanged;

    public bool IsOn { get; private set; }

    private void Awake()
    {
        IsOn = _startEnabled;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ChangeToggle);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ChangeToggle);
    }

    private void OnValidate()
    {
        if (_toggleOn == null || _toggleOff == null)
        {
            return;
        }

        UpdateToggleActivity();
    }

    private void ChangeToggle()
    {
        IsOn = !IsOn;
        UpdateToggleActivity();
        EnableChanged?.Invoke(IsOn);
    }

    private void UpdateToggleActivity()
    {
        _toggleOn.SetActive(_startEnabled);
        _toggleOff.SetActive(!_startEnabled);
    }
}
