using DG.Tweening;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Gameplay.Training
{
	public class TrainigSequenceIce : TrainigSequence
	{
	    [SerializeField] private CanvasGroup _iceHint;

	    private float _appearTime = 0.7f;
	    private float _disappearTime = 1.5f;


	    public override void StartTraining()
	    {
	        _iceHint.DOFade(1f, _appearTime);
	        _iceHint.transform.LookAt(MainCamera.transform);
	        UnitMovedEvent.UnitsMoved += OnUnitsMoved;
	    }

	    private void OnUnitsMoved(UnitsMoveInfo _)
	    {
	        _iceHint.DOFade(0f, _disappearTime);
	    }
	}
}
