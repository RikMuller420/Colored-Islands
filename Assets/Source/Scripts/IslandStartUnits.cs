using UnityEngine;

[System.Serializable]
public class IslandStartUnits
{
    [SerializeField] private Paint _paint;
    [SerializeField] private int _count;

    public IslandStartUnits(Paint paint, int count = 1)
    {
        _paint = paint;
        _count = count;
    }

    public Paint Paint { get => _paint; }
    public int Count { get => _count; }
}
