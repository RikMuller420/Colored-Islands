using System.Collections.Generic;
using UnityEngine;

public class TrainingMenuUpdater : MonoBehaviour
{
    [SerializeField] private List<GameObject> _trainingDummyPanels;
    [SerializeField] private List<GameObject> _unlockedPanels;
    [SerializeField] private RectTransform _customizationScrollView;

    private GameProgressStorage _progressStorage;
    private float _lockedCustomizationBottomOffset = 150f;
    private float _defaultCustomizationBottomOffset =120;

    public void Initialize(GameProgressStorage progressStorage)
    {
        _progressStorage = progressStorage;

        _progressStorage.TrainingFinished += UpdateMenuAvaliability;
        UpdateMenuAvaliability();
    }

    private void UpdateMenuAvaliability()
    {
        if (_progressStorage.IsTrainingFinished)
        {
            ApplyOffsetToCustomizationScrollView(_defaultCustomizationBottomOffset);
        }
        else
        {
            ApplyOffsetToCustomizationScrollView(_lockedCustomizationBottomOffset);
        }

        foreach (GameObject panel in _trainingDummyPanels)
        {
            panel.SetActive(_progressStorage.IsTrainingFinished == false);
        }

        foreach (GameObject panel in _unlockedPanels)
        {
            panel.SetActive(_progressStorage.IsTrainingFinished);
        }
    }

    private void ApplyOffsetToCustomizationScrollView(float offset)
    {
        _customizationScrollView.offsetMin = new Vector2(
            0f,
            offset);
    }
}
