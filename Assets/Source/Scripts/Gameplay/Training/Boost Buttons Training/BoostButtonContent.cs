using System.Collections.Generic;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Menu.Boosts;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Gameplay.Training
{
	[System.Serializable]
	public class BoostButtonContent
	{
	    [SerializeField] private BoostType _type;
	    [SerializeField] private BoostButton _buttonScript;
	    [SerializeField] private Button _button;
	    [SerializeField] private List<Image> _buttonImages;
	    [SerializeField] private GameObject _amountHolder;
	    [SerializeField] private RectTransform _rectTransform;

	    public BoostType Type => _type;
	    public BoostButton ButtonScript => _buttonScript;
	    public Button Button => _button;
	    public IReadOnlyCollection<Image> ButtonImages => _buttonImages;
	    public GameObject AmountHolder => _amountHolder;
	    public RectTransform RectTransform => _rectTransform;
	}
}
