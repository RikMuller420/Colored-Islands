using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AngryBar : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private AngryBarEmojiChanger _emojiAnimator;
    [SerializeField] private SmoothBarChanger _smoothBarChanger;
    [SerializeField] private Image _freezeImage;

    private float _freezeImageMaxAlpha = 0.7f;
    private float _freezeImageFadeDuration = 1f;
    private LevelProgressTracker _levelProgressTracker;
    private LevelLoader _levelLoader;
    private BoostAmountProvider _boostAmountProvider;
    private ObjectivesFreezeBoost _objectivesFreezeBoost;

    public void Initialize(LevelProgressTracker levelProgressTracker, LevelLoader levelLoader,
                           BoostAmountProvider boostAmountProvider, ObjectivesFreezeBoost objectivesFreezeBoost)
    {
        _levelProgressTracker = levelProgressTracker;
        _levelLoader = levelLoader;
        _boostAmountProvider = boostAmountProvider;
        _objectivesFreezeBoost = objectivesFreezeBoost;

        _boostAmountProvider.BoostApplyed += OnBoostApplyed;
        _objectivesFreezeBoost.BoostStopApplyed += OnFreezeBoostStoppedApplyed;
        _levelProgressTracker.AngryChanged += OnAngyValueChanged;
        _levelProgressTracker.LevelFinished += OnLevelFinished;
        _levelProgressTracker.AngryTaskFailed += OnAngryTaskFailed;
        _levelLoader.LevelChanged += OnLevelStarted;

        enabled = true;
    }

    private void OnAngyValueChanged(float value)
    {
        _emojiAnimator.UpdateEmojiAnimation(value);
        _smoothBarChanger.UpdateBarValue(value);
    }

    private void OnLevelFinished()
    {
        _emojiAnimator.PlayWinEmoji();
        _canvas.overrideSorting = true;
    }

    private void OnAngryTaskFailed()
    {
        _emojiAnimator.SetLooseEmoji();
        _smoothBarChanger.StopAnimation();
    }

    private void OnLevelStarted()
    {
        _canvas.overrideSorting = false;
        _emojiAnimator.ResetAnimator();
        _smoothBarChanger.StartAnimation();

        Color color = _freezeImage.color;
        color.a = 0;
        _freezeImage.color = color;
    }

    private void OnBoostApplyed(BoostType type)
    {
        _emojiAnimator.PlayBoostAnimation(type);

        if (type == BoostType.FreezeObjectives)
        {
            _freezeImage.DOFade(_freezeImageMaxAlpha, _freezeImageFadeDuration);
        }
    }

    private void OnFreezeBoostStoppedApplyed()
    {
        _emojiAnimator.StopFreezeBoostAnimation();
        _freezeImage.DOFade(0, _freezeImageFadeDuration);
    }
}
