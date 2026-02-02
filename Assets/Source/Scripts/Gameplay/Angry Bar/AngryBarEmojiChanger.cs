using System.Collections.Generic;
using SlimeGround.Gameplay.Boosts;
using UnityEngine;

namespace SlimeGround.Gameplay.AngryBar
{
	public class AngryBarEmojiChanger : MonoBehaviour
	{
	    private const string StartAnimationClip = "Start";
	    private const string LoseBool = "Is Losed";
	    private const string AngryFloat = "Angry Value";
	    private const string WinTrigger = "Win";
	    private const string BufferIslandTrigger = "Buffer Island Boost";
	    private const string FinishIslandTrigger = "Finish Island Boost";
	    private const string FreezeTrigger = "Freeze Boost";
	    private const string IsFreezedBool = "Is Freezed";
	    private const string ReducePaintTrigger = "Reduce Paint Boost";

	    [SerializeField] private Animator _emojiAnimator;

	    private Dictionary<BoostType, string> _boostTriggers;
	    private int _startAnimationHash;
	    private int _loseHash;
	    private int _angryValueHash;
	    private int _isFreezedHash;
	    
	    private void Awake()
	    {
	        _startAnimationHash = Animator.StringToHash(StartAnimationClip);
	        _loseHash = Animator.StringToHash(LoseBool);
	        _angryValueHash = Animator.StringToHash(AngryFloat);
	        _isFreezedHash = Animator.StringToHash(IsFreezedBool);

	        _boostTriggers = new Dictionary<BoostType, string>()
	        {
	            { BoostType.GrowBuferIsland, BufferIslandTrigger },
	            { BoostType.FinishIsland, FinishIslandTrigger },
	            { BoostType.FreezeObjectives, FreezeTrigger },
	            { BoostType.ReducePaints, ReducePaintTrigger }
	        };
	    }

	    public void ResetAnimator()
	    {
	        _emojiAnimator.SetBool(_loseHash, false);
	        _emojiAnimator.SetBool(_isFreezedHash, false);
	        _emojiAnimator.Play(_startAnimationHash);
	    }

	    public void PlayWinEmoji()
	    {
	        _emojiAnimator.SetTrigger(WinTrigger);
	    }

	    public void SetLooseEmoji()
	    {
	        _emojiAnimator.SetBool(_loseHash, true);
	    }

	    public void UpdateEmojiAnimation(float angryValue)
	    {
	        _emojiAnimator.SetFloat(_angryValueHash, angryValue);
	    }

	    public void PlayBoostAnimation(BoostType type)
	    {
	        if (type == BoostType.FreezeObjectives)
	        {
	            _emojiAnimator.SetBool(_isFreezedHash, true);
	        }

	        _emojiAnimator.SetTrigger(_boostTriggers[type]);
	    }

	    public void StopFreezeBoostAnimation()
	    {
	        _emojiAnimator.SetBool(_isFreezedHash, false);
	    }
	}
}
