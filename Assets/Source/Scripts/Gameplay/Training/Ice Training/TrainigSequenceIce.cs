using System.Collections;
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
		private float _cameraTrackDuration = 4f;
		private bool _isEventsSubscribed = false;

		private Coroutine _rotateCoroutine;
		private WaitForEndOfFrame _waitForEndOfFrame;

		private void OnDestroy()
		{
			if (_isEventsSubscribed)
			{
				UnitMovedEvent.UnitsMoved -= OnUnitsMoved;
				_isEventsSubscribed = false;
			}

			if (_rotateCoroutine != null)
			{
				StopCoroutine(_rotateCoroutine);
			}
		}

		public override void StartTraining()
	    {
			_waitForEndOfFrame = new WaitForEndOfFrame();
			_iceHint.DOFade(1f, _appearTime);
			_rotateCoroutine = StartCoroutine(RotateTextPanel());

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

		private IEnumerator RotateTextPanel()
		{
			float time = 0f;

			while (enabled)
			{
				yield return _waitForEndOfFrame;

				Vector3 direction = MainCamera.transform.position - _iceHint.transform.position;
				direction.x = 0f;
				_iceHint.transform.rotation = Quaternion.LookRotation(direction);

				time += Time.deltaTime;

				if (time > _cameraTrackDuration)
				{
					break;
				}
			}
		}
	}
}
