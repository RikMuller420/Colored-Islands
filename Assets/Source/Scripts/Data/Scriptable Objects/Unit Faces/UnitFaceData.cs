using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.UnitFaces
{
	[System.Serializable]
	public class UnitFaceData
	{
	    [SerializeField] private int _id;
	    [SerializeField] private Sprite _sprite;
	    [SerializeField] private Texture _texture;
	    [SerializeField] private Vector2 _tilling;
	    [SerializeField] private Vector2 _offset;
	    [SerializeField] private Material _material;
	    [SerializeField] private bool _isAviableOnStart;

	    public int Id => _id;
	    public Sprite Sprite => _sprite;
	    public Texture Texture => _texture;
	    public Vector2 Tilling => _tilling;
	    public Vector2 Offset => _offset;
	    public Material Material => _material;
	    public bool IsAviableOnStart => _isAviableOnStart;
	}
}
