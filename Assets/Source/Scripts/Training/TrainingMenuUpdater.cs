using System.Collections.Generic;
using UnityEngine;

public class TrainingMenuUpdater : MonoBehaviour
{
    [SerializeField] private List<GameObject> _trainingDummyPanels;
    [SerializeField] private List<GameObject> _unlockedPanels;

    private GameProgressStorage _progressStorage;

    public void Initialize(GameProgressStorage progressStorage)
    {
        _progressStorage = progressStorage;

        _progressStorage.TrainingFinished += UpdateMenuAvaliability;
        UpdateMenuAvaliability();
    }

    private void UpdateMenuAvaliability()
    {
        foreach (GameObject panel in _trainingDummyPanels)
        {
            panel.SetActive(!_progressStorage.IsTrainingFinished);
        }

        foreach (GameObject panel in _unlockedPanels)
        {
            panel.SetActive(_progressStorage.IsTrainingFinished);
        }
    }
}
