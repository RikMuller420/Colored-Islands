using UnityEngine;
using UnityEngine.UI;

public class ScrollViewArrowActivator : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private GameObject _topArrow;
    [SerializeField] private GameObject _botArrow;

    private float thresholder = 0.02f;
    private float topArrowValue;
    private float botArrowValue;

    private void Awake()
    {
        topArrowValue = 1f - thresholder;
        botArrowValue = 0f + thresholder;
        UpdateArrowActivivty(_scrollbar.value);
    }

    private void OnEnable()
    {
        _scrollbar.onValueChanged.AddListener(UpdateArrowActivivty);
    }

    private void OnDisable()
    {
        _scrollbar.onValueChanged.RemoveListener(UpdateArrowActivivty);
    }

    private void UpdateArrowActivivty(float scrollValue)
    {
        _topArrow.SetActive(scrollValue < topArrowValue);
        _botArrow.SetActive(scrollValue > botArrowValue);
    }
}
