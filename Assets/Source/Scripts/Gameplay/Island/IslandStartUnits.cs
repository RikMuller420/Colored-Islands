using UnityEngine;

[System.Serializable]
public class IslandStartUnits
{
    [SerializeField] private Paint _paint;
    [SerializeField] private int _amount;

    public IslandStartUnits(Paint paint, int count = 1)
    {
        _paint = paint;
        _amount = count;
    }

    public Paint Paint { get => _paint; }
    public int Amout { get => _amount; }
}
