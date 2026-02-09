using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Ads
{
	public class AddButton : MonoBehaviour
	{
	    [SerializeField] private Button _button;
	    [SerializeField] private TextMeshProUGUI _aviableText;
	    [SerializeField] private TextMeshProUGUI _collDownText;

	    private FreeStuffCollDownProvider _collDownProvider;

	    public event Action AviableClicked;

	    public void Initialize(FreeStuffCollDownProvider collDownProvider)
	    {
	        _collDownProvider = collDownProvider;
	        _button.onClick.AddListener(OnButtonClick);

	        _collDownProvider.CoolDownStarted += OnCoolDownStarted;
	        _collDownProvider.CoolDownFinished += OnCoolDownFinished;
	    }

		public void Dispose()
		{
			_collDownProvider.CoolDownStarted -= OnCoolDownStarted;
			_collDownProvider.CoolDownFinished -= OnCoolDownFinished;
		}

	    private void OnCoolDownStarted()
	    {
	        _aviableText.gameObject.SetActive(false);
	        _collDownText.gameObject.SetActive(true);
	    }

	    private void OnCoolDownFinished()
	    {
	        _aviableText.gameObject.SetActive(true);
	        _collDownText.gameObject.SetActive(false);
	    }

	    private void OnButtonClick()
	    {
	        if (_collDownProvider.TryUseAdd())
	        {
	            AviableClicked?.Invoke();
	        }
	    }
	}
}
