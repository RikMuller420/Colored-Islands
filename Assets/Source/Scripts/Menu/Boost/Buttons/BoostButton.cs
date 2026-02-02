using System;
using SlimeGround.Data.ScriptableObjects.Boosts;
using SlimeGround.Gameplay.Boosts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Boosts
{
	public class BoostButton : MonoBehaviour
	{
	    [SerializeField] protected Button Button;

	    [SerializeField] private Image _buttonBackground;
	    [SerializeField] private GameObject _glow;
	    [SerializeField] private TextMeshProUGUI _amountText;
	    [SerializeField] private BoostSettings _boostSettings;

	    private BoostAmountProvider _boostAmountProvider;
	    private BoostBuyConfirmationWindow _boostBuyWindow;
	    private Boost _boost;

	    public event Action TryBoostApplying;

	    public ButtonAnimator Animator { get; private set; }

	    private void OnEnable()
	    {
	        Button.onClick.AddListener(TryApplyBoost);
	        _boostAmountProvider.BoostsAmountChanged += OnBoostAmountChanged;
	    }

	    private void OnDisable()
	    {
	        Button.onClick.RemoveListener(TryApplyBoost);
	        _boostAmountProvider.BoostsAmountChanged -= OnBoostAmountChanged;
	    }

	    public void Initialize(Boost boost, BoostAmountProvider boostAmountProvider,
	                           BoostBuyConfirmationWindow boostBuyWindow)
	    {
	        _boost = boost;
	        _boostBuyWindow = boostBuyWindow;
	        _boostAmountProvider = boostAmountProvider;
	        Animator = new ButtonAnimator(_buttonBackground, _glow);
	        enabled = true;
	        UpdateBoostAmountText();
	    }

	    public void EnableInteractable()
	    {
	        Button.interactable = true;
	    }

	    public void DisableInteractable()
	    {
	        Button.interactable = false;
	    }

	    private void TryApplyBoost()
	    {
	        TryBoostApplying?.Invoke();

	        if (GetBoostAmount() == 0)
	        {
	            _boostBuyWindow.Open(_boost.Type);
	        }
	        else
	        {
	            _boost.TryApplyBoost();
	        }
	    }

	    private void OnBoostAmountChanged(BoostType boostType) => UpdateBoostAmountText();

	    private void UpdateBoostAmountText()
	    {
	        _amountText.text = GetBoostAmount().ToString();
	    }

	    private int GetBoostAmount()
	    {
	        return _boostAmountProvider.BoostAmount(_boost.Type);
	    }
	}
}
