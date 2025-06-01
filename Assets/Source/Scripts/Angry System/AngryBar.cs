using UnityEngine;

public class AngryBar : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private AngryBarEmojiChanger _emojiAnimator;
    [SerializeField] private SmoothBarChanger _smoothBarChanger;

    private float _value;
    private LevelProgressTracker _levelProgressTracker;
    private LevelLoader _levelLoader;


    public void Initialize(LevelProgressTracker levelProgressTracker, LevelLoader levelLoader)
    {
        _levelProgressTracker = levelProgressTracker;
        _levelLoader = levelLoader;

        _levelProgressTracker.AngryChanged += OnAngyValueChanged;
        _levelProgressTracker.LevelFinished += OnLevelFinished;
        _levelProgressTracker.LevelFailed += OnLevelFailed;
        _levelLoader.LevelChanged += OnLevelStarted;

        enabled = true;
    }

    private void OnAngyValueChanged(float value)
    {
        _value = value;
        _emojiAnimator.UpdateEmojiAnimation(value);
        _smoothBarChanger.UpdateBarValue(value);
    }

    private void OnLevelFinished()
    {
        _emojiAnimator.PlayWinEmoji();
        _canvas.overrideSorting = true;
    }

    private void OnLevelFailed()
    {
        _emojiAnimator.PlayLooseEmoji();
        _canvas.overrideSorting = true;
    }

    private void OnLevelStarted()
    {
        _canvas.overrideSorting = false;
    }
}
