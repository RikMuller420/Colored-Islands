using UnityEngine;
using UnityEngine.UI;

public class BoostButton : MonoBehaviour
{
    [SerializeField] protected Button Button;
    [SerializeField] private Image _buttonBackground;

    private Boost _boost;

    public ButtonAnimator Animator { get; private set; }

    private void OnEnable()
    {
        Button.onClick.AddListener(TryApplyBoost);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(TryApplyBoost);
    }

    public virtual void Initialize(Boost boost)
    {
        _boost = boost;
        Animator = new ButtonAnimator(_buttonBackground);
    }

    public void EnableInteractable()
    {
        Button.interactable = true;
    }

    public void DisableInteractable()
    {
        Button.interactable = false;
    }

    private void TryApplyBoost() => _boost.TryApplyBoost();
}
