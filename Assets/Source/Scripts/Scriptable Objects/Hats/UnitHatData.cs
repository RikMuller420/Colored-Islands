using UnityEngine;

[System.Serializable]
public class UnitHatData
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _selectSprite;
    [SerializeField] private Sprite _previewSprite;
    [SerializeField] private int _requredLevel;

    public int Id => _id;
    public Sprite SelectSprite => _selectSprite;
    public Sprite PreviewSprite => _previewSprite;
    public int RequredLevel => _requredLevel;
}
