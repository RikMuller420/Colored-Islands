using System.Linq;
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
        LevelProgress level = _progressStorage.FirstUnfinishedLevel;

        if (level == null)
        {
            level = _progressStorage.Levels.Last();
        }

        _levelText.text = $"Next Level: {level.Id:D3}";
    }
}
