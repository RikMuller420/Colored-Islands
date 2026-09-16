using SlimeGround.Data.Saves;
using TMPro;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Roulette
{
	public class AviableSpinCountView : MonoBehaviour
	{
	    [SerializeField] private TextMeshProUGUI _text;

	    private IPlayerData _playerData;

	    public void Initialize(IPlayerData playerData)
	    {
	        _playerData = playerData;
	        _playerData.Resources.SpinCountChanged += UpdateViewText;
	        UpdateViewText();
	    }

		public void Dispose()
		{
			_playerData.Resources.SpinCountChanged -= UpdateViewText;
		}

		private void UpdateViewText()
	    {
	        _text.text = _playerData.Resources.AviableSpinCount.ToString();
	    }
	}
}
