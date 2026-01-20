using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AngryBarView : MonoBehaviour
{
    [SerializeField] private LevelProgressTracker _levelProgressTracker;
    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private AngryBarEmojiChanger _emojiAnimator;
    [SerializeField] private SmoothBarChanger _smoothBarChanger;
    [SerializeField] private Image _freezeImage;

    private float _freezeImageMaxAlpha = 0.7f;
    private float _freezeImageFadeDuration = 1f;
    private BoostAmountProvider _boostAmountProvider;
    private IBoostStopApplyedEvent _freezeBoostApplyed;

    public void Initialize(BoostAmountProvider boostAmountProvider, IBoostStopApplyedEvent freezeBoostApplyed)
    {
        _boostAmountProvider = boostAmountProvider;
        _freezeBoostApplyed = freezeBoostApplyed;

        _boostAmountProvider.BoostApplyed += OnBoostApplyed;
        _freezeBoostApplyed.StopApplyed += OnFreezeBoostStoppedApplyed;
        _levelProgressTracker.AngryChanged += OnAngyValueChanged;
        _levelProgressTracker.LevelFinished += OnLevelFinished;
        _levelProgressTracker.AngryTaskFailed += OnAngryTaskFailed;
        _levelChangeEventTracker.LevelChanged += OnLevelChanged;

        enabled = true;
    }

    private void OnAngyValueChanged(float value)
    {
        _emojiAnimator.UpdateEmojiAnimation(value);
        _smoothBarChanger.UpdateBarValue(value);
    }

    private void OnLevelFinished(ILevelData _)
    {
        _emojiAnimator.PlayWinEmoji();
        _canvas.overrideSorting = true;
    }

    private void OnAngryTaskFailed()
    {
        _emojiAnimator.SetLooseEmoji();
        _smoothBarChanger.StopAnimation();
    }

    private void OnLevelChanged(ILevelData _)
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
