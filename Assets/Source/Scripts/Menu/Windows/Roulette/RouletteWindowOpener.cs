using DG.Tweening;
using SlimeGround.Data.Saves;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Roulette
{
	public class RouletteWindowOpener : MonoBehaviour
	{
	    [SerializeField] private Button _button;
	    [SerializeField] private RouletteWindow _window;
	    [SerializeField] private CanvasGroup _canvasGroup;

	    private IPlayerData _playerData;
	    private float _fadeDuration = 1f;
	    private bool _isAvailable = false;

	    private void OnEnable()
	    {
	        _button.onClick.AddListener(OpenWindow);
	    }

	    private void OnDisable()
	    {
	        _button.onClick.RemoveListener(OpenWindow);
	    }

	    public void Initialize(IPlayerData playerData)
	    {
	        _playerData = playerData;
	        _playerData.SpinCountChanged += UpdateButtonAviability;
	        UpdateButtonAviability();
	    }

	    private void UpdateButtonAviability()
	    {
	        bool isAvailable = _playerData.AviableSpinCount > 0;

	        if (isAvailable == _isAvailable)
	        {
	            return;
	        }

	        _isAvailable = isAvailable;
	        _button.interactable = isAvailable;
	        float alpha = isAvailable ? 1f : 0;
	        _canvasGroup.DOFade(alpha, _fadeDuration)
	                       .SetEase(Ease.OutQuad);
	    }

	    private void OpenWindow()
	    {
	        _window.Open();
	    }
	}
}
