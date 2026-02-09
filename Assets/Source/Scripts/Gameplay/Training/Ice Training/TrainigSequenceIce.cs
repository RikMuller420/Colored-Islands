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
		private bool _isEventsSubscribed = false;

		private void OnDestroy()
		{
			if (_isEventsSubscribed)
			{
				UnitMovedEvent.UnitsMoved -= OnUnitsMoved;
				_isEventsSubscribed = false;
			}
		}	

		public override void StartTraining()
	    {
	        _iceHint.DOFade(1f, _appearTime);
	        _iceHint.transform.LookAt(MainCamera.transform);

			if (_isEventsSubscribed == false)
			{
				UnitMovedEvent.UnitsMoved += OnUnitsMoved;
				_isEventsSubscribed = true;
			}
	    }

	    private void OnUnitsMoved(UnitsMoveInfo _)
	    {
	        _iceHint.DOFade(0f, _disappearTime);
	    }
	}
}
