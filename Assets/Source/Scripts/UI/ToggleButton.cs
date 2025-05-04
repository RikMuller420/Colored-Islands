using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private bool _startEnabled = true;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _toggleOn;
    [SerializeField] private GameObject _toggleOff;

    private bool _isOn;

    public event Action<bool> EnableChanged;

    private void Awake()
    {
        _isOn = _startEnabled;
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
        _isOn = !_isOn;
        UpdateToggleActivity();
        EnableChanged?.Invoke(_isOn);
    }

    private void UpdateToggleActivity()
    {
        _toggleOn.SetActive(_startEnabled);
        _toggleOff.SetActive(!_startEnabled);
    }
}
