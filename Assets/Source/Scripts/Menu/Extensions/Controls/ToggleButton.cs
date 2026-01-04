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
        _button.onClick.AddListener(SwitchToggle);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SwitchToggle);
    }

    private void OnValidate()
    {
        if (_toggleOn == null || _toggleOff == null)
        {
            return;
        }

        _isOn = _startEnabled;
        UpdateToggleView();
    }

    public void SetToggle(bool isOn)
    {
        _isOn = isOn;
        UpdateToggleView();
    }

    private void SwitchToggle()
    {
        _isOn = !_isOn;
        UpdateToggleView();
        EnableChanged?.Invoke(_isOn);
    }

    private void UpdateToggleView()
    {
        _toggleOn.SetActive(_isOn);
        _toggleOff.SetActive(!_isOn);
    }
}
