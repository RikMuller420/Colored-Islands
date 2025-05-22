using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabSystem
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _activeBackground;
        [SerializeField] private Image _inactiveBackground;
        [SerializeField] private TextMeshProUGUI _text;

        private Color _activeTextColor = Color.white;
        private Color _inactiveTextColor = new Color(0.82f, 0.5f, 0.15f);
        private bool _isActive = false;

        public event Action<TabButton> TabSelected;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            TabSelected?.Invoke(this);
        }

        public void SetActive()
        {
            _isActive = true;
            UpdateActiveState();
        }

        public void SetInactive()
        {
            _isActive = false;
            UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            _button.enabled = !_isActive;
            _activeBackground.enabled = _isActive;
            _inactiveBackground.enabled = !_isActive;
            _text.color = _isActive ? _activeTextColor : _inactiveTextColor;
        }
    }
}
