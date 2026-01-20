using System.Collections;
using UnityEngine;

public class UnitsMoveSoundPlayer : MonoBehaviour
{
    private IUnitMovedEvent _unitMover;
    private AudioSource _moveSound;

    private WaitForSeconds _wait;
    private float _delay = 0.1f;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    public void Initialize(IUnitMovedEvent unitMover, AudioSource moveSound)
    {
        _unitMover = unitMover;
        _moveSound = moveSound;

        _unitMover.UnitsMoved += OnUnitMoved;
        enabled = true;
    }

    private void OnUnitMoved(UnitsMoveInfo unitsMoveInfo)
    {
        StartCoroutine(PlayMoveSound(unitsMoveInfo.Units.Count));
    }

    public IEnumerator PlayMoveSound(int soundCount)
    {
        for (int i = 0; i < soundCount; i++)
        {
            _moveSound.Play();

            yield return _wait;
        }
    }
}
