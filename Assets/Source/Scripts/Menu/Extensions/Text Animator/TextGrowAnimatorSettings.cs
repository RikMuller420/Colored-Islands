using UnityEngine;

namespace SlimeGround.Menu.Extensions.TextAnimator
{
	[System.Serializable]
	public class TextGrowAnimatorSettings
	{
	    [SerializeField] private float _growAnimationDuration = 1f;
	    [SerializeField] private int _pulseCycles = 8;
	    [SerializeField] private float _pulseFrequency = 0.15f;
	    [SerializeField] private float _pulseMinSize = 1f;
	    [SerializeField] private float _pulseMaxSize = 1.15f;
	    [SerializeField] private float _animationDelay = 0f;

	    public float GrowAnimationDuration { get => _growAnimationDuration; }
	    public int PulseCycles { get => _pulseCycles; }
	    public float PulseFrequency { get => _pulseFrequency; }
	    public float PulseMinSize { get => _pulseMinSize; }
	    public float PulseMaxSize { get => _pulseMaxSize; }
	    public float AnimationDelay { get => _animationDelay; }
	}
}
