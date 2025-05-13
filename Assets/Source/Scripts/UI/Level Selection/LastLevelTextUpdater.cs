using System.Linq;
using Lean.Localization;
using UnityEngine;

public class LastLevelTextUpdater : MonoBehaviour
{
    [SerializeField] private LeanToken _levelNumberToken;

    private GameProgressStorage _progressStorage;

    private void OnEnable()
    {
        _progressStorage.LevelProgressChanged += UpdateLevelNumberToken;
    }

    private void OnDisable()
    {
        _progressStorage.LevelProgressChanged -= UpdateLevelNumberToken;
    }

    public void Initialize(GameProgressStorage progressStorage)
    {
        _progressStorage = progressStorage;
        UpdateLevelNumberToken();
        enabled = true;
    }

    private void UpdateLevelNumberToken()
    {
        LevelProgress level = _progressStorage.FirstUnfinishedLevel;

        if (level == null)
        {
            level = _progressStorage.Levels.Last();
        }

        _levelNumberToken.SetValue($"{level.Id:D3}");
    }
}
