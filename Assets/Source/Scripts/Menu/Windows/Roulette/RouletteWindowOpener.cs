using DG.Tweening;
using SlimeGround.Data.Saves;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Roulette
{
	public class RouletteWindowOpener : MonoBehaviour
	{
		private const float FadeDuration = 1f;

		[SerializeField] private Button _button;
	    [SerializeField] private RouletteWindow _window;
	    [SerializeField] private CanvasGroup _canvasGroup;

	    private IPlayerData _playerData;
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
	        _playerData.Resources.SpinCountChanged += UpdateButtonAviability;
	        UpdateButtonAviability();
	    }

	    private void UpdateButtonAviability()
	    {
	        bool isAvailable = _playerData.Resources.AviableSpinCount > 0;

	        if (isAvailable == _isAvailable)
	        {
	            return;
	        }

	        _isAvailable = isAvailable;
	        _button.interactable = isAvailable;
	        float alpha = isAvailable ? 1f : 0;
	        _canvasGroup.DOFade(alpha, FadeDuration)
	                       .SetEase(Ease.OutQuad);
	    }

	    private void OpenWindow()
	    {
	        _window.Open();
	    }
	}
}
