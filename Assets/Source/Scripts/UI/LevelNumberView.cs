using TMPro;
using UnityEngine;

public class LevelNumberView : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private TextMeshProUGUI _numberText;

    private void OnEnable()
    {
        _levelLoader.LevelChanged += OnLevelChanged;   
    }

    private void OnDisable()
    {
        _levelLoader.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged()
    {
        _numberText.text = $"{_levelLoader.CurrentLevelData.Id:D3}";
    }
}
