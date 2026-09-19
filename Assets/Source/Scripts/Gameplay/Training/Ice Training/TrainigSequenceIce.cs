using System.Collections;
using DG.Tweening;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Gameplay.Training
{
	public class TrainigSequenceIce : TrainigSequence
	{
		private const float AppearTime = 0.7f;
		private const float DisappearTime = 1.5f;
		private const float CameraTrackDuration = 4f;

		[SerializeField] private CanvasGroup _iceHint;

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
			_iceHint.DOFade(1f, AppearTime);
			_rotateCoroutine = StartCoroutine(RotateTextPanel());

			if (_isEventsSubscribed == false)
			{
				UnitMovedEvent.UnitsMoved += OnUnitsMoved;
				_isEventsSubscribed = true;
			}
	    }

	    private void OnUnitsMoved(UnitsMoveInfo _)
	    {
	        _iceHint.DOFade(0f, DisappearTime);
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

				if (time > CameraTrackDuration)
				{
					break;
				}
			}
		}
	}
}
