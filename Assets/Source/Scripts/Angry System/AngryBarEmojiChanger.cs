using UnityEngine;

public class AngryBarEmojiChanger : MonoBehaviour
{
    [SerializeField] private AngryBar _angryBar;
    [SerializeField] private Animator _emojiAnimator;

    private float _animationZoneWidth = 0.1f;
    private int _currentAnimationZone = 0;
    private float _animationZoneThreshold = 0.02f;
    private string[] _emojiTriggers = new string[]
    {
        "BigSmile",
        "SmallSmile",
        "Neutral",
        "Sad",
        "Panic",
        "Rage",
        "BigRage"
    };

    private void Awake()
    {
        _animationZoneWidth = 1f / _emojiTriggers.Length;
        TryUpdateEmojiAnimation();
    }
    private void OnEnable()
    {
        _angryBar.Changed += TryUpdateEmojiAnimation;
    }

    private void OnDisable()
    {
        _angryBar.Changed -= TryUpdateEmojiAnimation;
    }

    private void TryUpdateEmojiAnimation()
    {
        int newZoneIndex = Mathf.FloorToInt(_angryBar.Value / _animationZoneWidth);

        if (newZoneIndex != _currentAnimationZone)
        {
            float zoneBoundary = newZoneIndex * _animationZoneWidth;
            bool isAnimationChangeRequire = false;

            if (newZoneIndex > _currentAnimationZone)
            {
                isAnimationChangeRequire = _angryBar.Value >= zoneBoundary + _animationZoneThreshold;
            }
            else if (newZoneIndex < _currentAnimationZone)
            {
                isAnimationChangeRequire = _angryBar.Value <= zoneBoundary + _animationZoneWidth - _animationZoneThreshold;
            }

            if (isAnimationChangeRequire)
            {
                _emojiAnimator.SetTrigger(_emojiTriggers[newZoneIndex]);
                _currentAnimationZone = newZoneIndex;
            }
        }
    }
}
