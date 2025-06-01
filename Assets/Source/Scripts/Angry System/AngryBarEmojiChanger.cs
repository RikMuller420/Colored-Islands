using UnityEngine;

public class AngryBarEmojiChanger : MonoBehaviour
{
    [SerializeField] private Animator _emojiAnimator;

    private float _animationZoneWidth = 0.1f;
    private int _currentAnimationZone = 0;
    private float _animationZoneThreshold = 0.02f;
    private string[] _angryLineTriggers = new string[]
    {
        "BigSmile",
        "SmallSmile",
        "Neutral",
        "Sad",
        "Panic",
        "Rage",
        "BigRage"
    };
    private string _winTrigger = "Win";
    private string _loseTrigger = "Lose";

    private void Awake()
    {
        _animationZoneWidth = 1f / _angryLineTriggers.Length;
    }

    public void PlayWinEmoji()
    {
        _emojiAnimator.SetTrigger(_winTrigger);
        _currentAnimationZone = -1;
    }

    public void PlayLooseEmoji()
    {
        _emojiAnimator.SetTrigger(_loseTrigger);
        _currentAnimationZone = -1;
    }

    public void UpdateEmojiAnimation(float angryValue)
    {
        int newZoneIndex = Mathf.FloorToInt(angryValue / _animationZoneWidth);

        if (newZoneIndex != _currentAnimationZone)
        {
            float zoneBoundary = newZoneIndex * _animationZoneWidth;
            bool isAnimationChangeRequire = false;

            if (newZoneIndex > _currentAnimationZone)
            {
                isAnimationChangeRequire = angryValue >= zoneBoundary + _animationZoneThreshold;
            }
            else if (newZoneIndex < _currentAnimationZone)
            {
                isAnimationChangeRequire = angryValue <= zoneBoundary + _animationZoneWidth - _animationZoneThreshold;
            }

            if (isAnimationChangeRequire)
            {
                _emojiAnimator.SetTrigger(_angryLineTriggers[newZoneIndex]);
                _currentAnimationZone = newZoneIndex;
            }
        }
    }
}
