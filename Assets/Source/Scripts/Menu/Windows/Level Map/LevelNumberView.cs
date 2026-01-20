using TMPro;
using UnityEngine;

public class LevelNumberView : MonoBehaviour
{
    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
    [SerializeField] private TextMeshProUGUI _numberText;

    private void OnEnable()
    {
        _levelChangeEventTracker.LevelChanged += OnLevelChanged;   
    }

    private void OnDisable()
    {
        _levelChangeEventTracker.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged(ILevelData levelData)
    {
        _numberText.text = $"{levelData.LevelId:D3}";
    }
}
