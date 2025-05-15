using UnityEngine;
using UnityEngine.UI;

public class PaintAmountReduceBoostButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private PaintAmountReduceBoost _paintAmountReduceBoost;

    private void OnEnable()
    {
        _button.onClick.AddListener(ApplyBoost);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ApplyBoost);
    }

    public void Initialize(PaintAmountReduceBoost paintAmountReduceBoost)
    {
        _paintAmountReduceBoost = paintAmountReduceBoost;
    }

    private void ApplyBoost()
    {
        _paintAmountReduceBoost.ReduceColorAmount();
    }
}
