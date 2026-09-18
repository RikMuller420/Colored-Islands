using System.Collections;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Effects.Sound
{
	public class UnitsMoveSoundPlayer : MonoBehaviour
	{
		private const float AwakeDelay = 0.1f;

		private IUnitMovedEvent _unitMover;
	    private AudioSource _moveSound;

	    private WaitForSeconds _wait;

	    private void Awake()
	    {
	        _wait = new WaitForSeconds(AwakeDelay);
	    }

	    public void Initialize(IUnitMovedEvent unitMover, AudioSource moveSound)
	    {
	        _unitMover = unitMover;
	        _moveSound = moveSound;

	        _unitMover.UnitsMoved += OnUnitMoved;
	        enabled = true;
	    }

		public void Dispose()
		{
			_unitMover.UnitsMoved -= OnUnitMoved;
		}

		private void OnUnitMoved(UnitsMoveInfo unitsMoveInfo)
	    {
	        StartCoroutine(PlayMoveSound(unitsMoveInfo.Units.Count));
	    }

		private IEnumerator PlayMoveSound(int soundCount)
	    {
	        for (int i = 0; i < soundCount; i++)
	        {
	            _moveSound.Play();

	            yield return _wait;
	        }
	    }
	}
}
