using UnityEngine;

[System.Serializable]
public class Level1TrainingMove
{
    [SerializeField] private Level1TrainingMoveType _type;
    [SerializeField] private Island _island;
    [SerializeField] private bool _isUseBufferIsland;
    [SerializeField] private Paint _unitsPaint;
    [SerializeField] private RectTransform _pointerPosition;

    public Level1TrainingMoveType Type => _type;
    public Island Island => _island;
    public bool IsUseBufferIsland => _isUseBufferIsland;
    public Paint UnitsPaint => _unitsPaint;
    public RectTransform PointerPosition => _pointerPosition;
}
