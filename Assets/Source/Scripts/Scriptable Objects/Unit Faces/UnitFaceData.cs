using UnityEngine;

[System.Serializable]
public class UnitFaceData
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Material _material;
    [SerializeField] private bool _isAviableOnStart;

    public int Id => _id;
    public Sprite Sprite => _sprite;
    public Material Material => _material;
    public bool IsAviableOnStart => _isAviableOnStart;
}
