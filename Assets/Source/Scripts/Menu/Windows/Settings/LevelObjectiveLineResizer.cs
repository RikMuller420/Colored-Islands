using TMPro;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Settings
{
	public class LevelObjectiveLineResizer : MonoBehaviour
	{
	    [SerializeField] private TextMeshProUGUI _text;
	    [SerializeField] private RectTransform _icon;

	    private void Update()
	    {
	        _icon.offsetMin = new Vector2(-_text.preferredWidth, 0f);
	    }
	}
}
