using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Extensions.ScrollViewArrow
{

	public class ScrollViewArrowActivator : MonoBehaviour
	{
	    [SerializeField] private Scrollbar _scrollbar;
	    [SerializeField] private GameObject _topArrow;
	    [SerializeField] private GameObject _botArrow;
	    [SerializeField] private CanvasGroup _shadowTop;
	    [SerializeField] private CanvasGroup _shadowBot;
	    [SerializeField] private RectTransform _content;
	    [SerializeField] private RectTransform _window;

	    private float _thresholder = 0.02f;
	    private float _topArrowValue;
	    private float _botArrowValue;
	    private bool _isUpdating = false;


	    private void Awake()
	    {
	        _topArrowValue = 1f - _thresholder;
	        _botArrowValue = 0f + _thresholder;
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
	        if (_isUpdating)
	        {
	            return;
	        }

	        StartCoroutine(UpdateArrowActivityCoroutine(scrollValue));
	    }

	    private IEnumerator UpdateArrowActivityCoroutine(float scrollValue)
	    {
	        _isUpdating = true;

	        yield return new WaitForEndOfFrame(); // ���� ����� �����, ����� �������� ��������� � UI rebuild

	        if (_content.rect.height < _window.rect.height)
	        {
	            _topArrow.SetActive(false);
	            _botArrow.SetActive(false);

	            _shadowTop.alpha = 0f;
	            _shadowBot.alpha = 0f;
	        }
	        else
	        {
	            _topArrow.SetActive(scrollValue < _topArrowValue);
	            _botArrow.SetActive(scrollValue > _botArrowValue);

	            _shadowTop.alpha = Mathf.Lerp(1f, 0f, (scrollValue - _topArrowValue) / _thresholder);
	            _shadowBot.alpha = Mathf.Lerp(0f, 1f, (scrollValue - _botArrowValue) / _thresholder);
	        }

	        _isUpdating = false;
	    }
	}

}
