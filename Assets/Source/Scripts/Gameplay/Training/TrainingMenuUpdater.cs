using System.Collections.Generic;
using UnityEngine;

public class TrainingMenuUpdater : MonoBehaviour
{
    [SerializeField] private List<GameObject> _trainingDummyPanels;
    [SerializeField] private List<GameObject> _unlockedPanels;
    [SerializeField] private RectTransform _customizationScrollView;

    private IPlayerData _playerData;

    private float _lockedCustomizationBottomOffset = 150f;
    private float _defaultCustomizationBottomOffset = 0;

    public void Initilize(IPlayerData playerData)
    {
        _playerData = playerData;

        _playerData.TrainingFinished += UpdateMenuAvaliability;
        UpdateMenuAvaliability();
    }

    private void UpdateMenuAvaliability()
    {
        if (_playerData.IsTrainingFinished)
        {
            ApplyOffsetToCustomizationScrollView(_defaultCustomizationBottomOffset);
        }
        else
        {
            ApplyOffsetToCustomizationScrollView(_lockedCustomizationBottomOffset);
        }

        foreach (GameObject panel in _trainingDummyPanels)
        {
            panel.SetActive(_playerData.IsTrainingFinished == false);
        }

        foreach (GameObject panel in _unlockedPanels)
        {
            panel.SetActive(_playerData.IsTrainingFinished);
        }
    }

    private void ApplyOffsetToCustomizationScrollView(float offset)
    {
        _customizationScrollView.offsetMin = new Vector2(
            0f,
            offset);
    }
}
