using UnityEngine;
using UnityEngine.UI;

public class IslandFinishBoostButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private IslandFinishBoost _islandFinishBoost;

    private void OnEnable()
    {
        _button.onClick.AddListener(ApplyBoost);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ApplyBoost);
    }

    public void Initialize(IslandFinishBoost islandFinishBoost)
    {
        _islandFinishBoost = islandFinishBoost;
    }

    private void ApplyBoost()
    {
        _islandFinishBoost.StartBoostApplying();
    }
}
