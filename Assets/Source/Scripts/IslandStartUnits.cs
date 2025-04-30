using UnityEngine;

[System.Serializable]
public class IslandStartUnits
{
    [SerializeField] private Paint _paint;
    [SerializeField] private int _count;

    public Paint Paint { get => _paint; }
    public int Count { get => _count; }
}
