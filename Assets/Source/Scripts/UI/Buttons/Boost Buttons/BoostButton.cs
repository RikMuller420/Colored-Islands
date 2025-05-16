using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostButton : MonoBehaviour
{
    [SerializeField] protected Button Button;
    [SerializeField] private Image _buttonBackground;
    [SerializeField] private TextMeshProUGUI _amountText;
    
    private BoostAmountProvider _boostAmountProvider;
    private Boost _boost;

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

    public virtual void Initialize(Boost boost, BoostAmountProvider boostAmountProvider)
    {
        _boost = boost;
        _boostAmountProvider = boostAmountProvider;
        Animator = new ButtonAnimator(_buttonBackground);
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
        if (GetBoostAmount() == 0)
        {
            //Открыть окно магазина
        }
        else
        {
            _boost.TryApplyBoost();
        }
    }

    private void OnBoostAmountChanged() => UpdateBoostAmountText();

    private void UpdateBoostAmountText()
    {
        _amountText.text = GetBoostAmount().ToString();
    }

    private int GetBoostAmount()
    {
        Type boostType = _boost.GetType();
        MethodInfo getBoostAmount = _boostAmountProvider.GetType()
                            .GetMethod(nameof(_boostAmountProvider.BoostAmount))
                            .MakeGenericMethod(boostType);

        return (int)getBoostAmount.Invoke(_boostAmountProvider, null);
    }
}
