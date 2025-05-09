using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovesObjectiveView : MonoBehaviour
{
    [SerializeField] private Image _moveFiller;
    [SerializeField] private TextMeshProUGUI _restAviableMovesText;
    [SerializeField] private TextMeshProUGUI _movesForExtraStarText;
    [SerializeField] private RectTransform _movesForExtraStarPanel;
    [SerializeField] private PanelAnimator _movesPanelAnimator;

    private LevelSettingsData _levelData;
    private bool _isMovePanelDropped = false;

    public void ResetPanel(LevelSettingsData levelData)
    {
        _levelData = levelData;

        _moveFiller.fillAmount = 1f;
        _restAviableMovesText.text = _levelData.LevelMoveLimit.ToString();
        _movesForExtraStarText.text = _levelData.ExtraStarMoveLimit.ToString();

        PlaceMovesPanel();

        _movesPanelAnimator.ResetAnimator();
        _isMovePanelDropped = false;
    }

    public void OnMovesChanged(int moves)
    {
        _restAviableMovesText.text = (_levelData.LevelMoveLimit - moves).ToString();

        float restMovesFillAmount = 1f - (float)moves / _levelData.LevelMoveLimit;
        _moveFiller.fillAmount = restMovesFillAmount;

        if (_isMovePanelDropped == false && moves > _levelData.ExtraStarMoveLimit)
        {
            _isMovePanelDropped = true;
            _movesPanelAnimator.DropPanel();
        }
    }

    public void StopShaking()
    {
        _movesPanelAnimator.StopShaking();
    }

    private void PlaceMovesPanel()
    {
        float anchorPositionX = 1f - (float)_levelData.ExtraStarMoveLimit / _levelData.LevelMoveLimit;

        Vector2 anchorMin = _movesForExtraStarPanel.anchorMin;
        Vector2 anchorMax = _movesForExtraStarPanel.anchorMax;

        anchorMin.x = anchorPositionX;
        anchorMax.x = anchorPositionX;

        _movesForExtraStarPanel.anchorMin = anchorMin;
        _movesForExtraStarPanel.anchorMax = anchorMax;
    }
}
