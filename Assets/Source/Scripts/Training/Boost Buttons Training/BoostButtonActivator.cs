using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BoostButtonActivator : MonoBehaviour
{
    [SerializeField] private List<BoostButtonContent> _boostButtons = new();

    private float _fadeDuration = 0.7f;

    public BoostButton GetBoostButton(BoostType boostType) =>
        _boostButtons.Find(button => button.Type == boostType).Button;

    public void DeactivateAllButtons()
    {
        foreach (BoostButtonContent button in _boostButtons)
        {
            button.Button.gameObject.SetActive(false);
            button.AmountHolder.SetActive(false);
        }
    }

    public void ActivateAllButtons()
    {
        foreach (BoostButtonContent button in _boostButtons)
        {
            button.Button.gameObject.SetActive(true);
            button.AmountHolder.SetActive(true);
        }
    }

    public void ActivateButtonImmediate(BoostType type)
    {
        BoostButtonContent boostButton = _boostButtons.Find(button => button.Type == type);
        boostButton.Button.gameObject.SetActive(true);
    }

    public void ActivateButtonWithFade(BoostType type)
    {
        BoostButtonContent boostButton = _boostButtons.Find(button => button.Type == type);
        boostButton.Button.gameObject.SetActive(true);

        foreach (Image image in boostButton.ButtonImages)
        {
            Color initialColor = image.color;
            initialColor.a = 0f;
            image.color = initialColor;

            image.DOFade(1f, _fadeDuration).SetEase(Ease.InOutSine);
        }
    }
}
