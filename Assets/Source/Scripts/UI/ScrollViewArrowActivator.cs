using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewArrowActivator : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private GameObject _topArrow;
    [SerializeField] private GameObject _botArrow;
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _window;

    private float _thresholder = 0.02f;
    private float _topArrowValue;
    private float _botArrowValue;
    private float _initializationDelay = 0.1f;

    private void Awake()
    {
        _topArrowValue = 1f - _thresholder;
        _botArrowValue = 0f + _thresholder;
        DelayedInitialization();
    }

    private void OnEnable()
    {
        _scrollbar.onValueChanged.AddListener(UpdateArrowActivivty);
    }

    private void OnDisable()
    {
        _scrollbar.onValueChanged.RemoveListener(UpdateArrowActivivty);
    }

    private IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(_initializationDelay);

        UpdateArrowActivivty(_scrollbar.value);
    }

    private void UpdateArrowActivivty(float scrollValue)
    {
        if (_content.rect.height < _window.rect.height)
        {
            _topArrow.SetActive(false);
            _botArrow.SetActive(false);

            return;
        }

        _topArrow.SetActive(scrollValue < _topArrowValue);
        _botArrow.SetActive(scrollValue > _botArrowValue);
    }
}
