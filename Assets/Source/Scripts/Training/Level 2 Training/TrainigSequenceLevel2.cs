using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainigSequenceLevel2 : TrainigSequence
{
    [SerializeField] private RectTransform _pointer;
    [SerializeField] private Image _pointerImage;

    public override void StartTraining()
    {
        BoostButtonActivator.DeactivateAllButtons();

    }
}
