using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    private List<string> _textVariants;
    private int _value;

    public event Action<int> ValueChanged;

    private void OnEnable()
    {
        _leftButton.onClick.AddListener(GoToPreviousVariant);
        _rightButton.onClick.AddListener(GoToNextVariant);
    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(GoToPreviousVariant);
        _rightButton.onClick.RemoveListener(GoToNextVariant);
    }

    public void Initialize(IEnumerable<string> textVariants, int startValue = 0)
    {
        _textVariants = new List<string>(textVariants);

        if (startValue < 0 || startValue >= _textVariants.Count)
        {
            throw new InvalidOperationException("Index out of range");
        }

        _value = startValue;
        UpdateText();
    }

    private void GoToNextVariant()
    {
        _value = ++_value % _textVariants.Count;
        UpdateText();
        ValueChanged?.Invoke(_value);
    }

    private void GoToPreviousVariant()
    {
        _value--;

        if (_value < 0)
        {
            _value = _textVariants.Count - 1;
        }

        UpdateText();
        ValueChanged?.Invoke(_value);
    }

    private void UpdateText()
    {
        _textMesh.text = _textVariants[_value];
    }
}
