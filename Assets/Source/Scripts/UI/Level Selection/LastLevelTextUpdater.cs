using TMPro;
using UnityEngine;

public class LastLevelTextUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    private GameProgressStorage _progressStorage;

    private void OnEnable()
    {
        _progressStorage.LevelProgressChanged += UpdateLevelText;
    }

    private void OnDisable()
    {
        _progressStorage.LevelProgressChanged -= UpdateLevelText;
    }

    public void Initialize(GameProgressStorage progressStorage)
    {
        _progressStorage = progressStorage;
        UpdateLevelText();
        enabled = true;
    }

    private void UpdateLevelText()
    {
        int levelId = _progressStorage.FirstUnfinishedLevel.Id;
        _levelText.text = $"Last Level: {levelId:D3}";
    }
}
