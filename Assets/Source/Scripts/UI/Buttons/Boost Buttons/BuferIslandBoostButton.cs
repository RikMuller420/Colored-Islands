using UnityEngine;
using UnityEngine.UI;

public class BuferIslandBoostButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private BufferIslandBoost _bufferIslandBoost;

    private void OnEnable()
    {
        _button.onClick.AddListener(ApplyBoost);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ApplyBoost);
    }

    public void Initialize(BufferIslandBoost bufferIslandBoost)
    {
        _bufferIslandBoost = bufferIslandBoost;
        enabled = true;
    }

    private void ApplyBoost()
    {
        _bufferIslandBoost.BoostIslandsSize();
    }
}
