using System.Threading.Tasks;
using UnityEngine;

public class UnitsMoveSoundPlayer
{
    private UnitMover _unitMover;
    private AudioSource _moveSound;

    private int _delay = 100;

    public UnitsMoveSoundPlayer(UnitMover unitMover, AudioSource moveSound)
    {
        _unitMover = unitMover;
        _moveSound = moveSound;

        _unitMover.UnitsMoved += PlayMoveSound;
    }

    public async void PlayMoveSound(UnitsMoveInfo unitsMoveInfo)
    {
        for (int i = 0; i < unitsMoveInfo.Units.Count; i++)
        {
            _moveSound.Play();

            await Task.Delay(_delay);
        }
    }
}
