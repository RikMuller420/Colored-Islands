using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Hats
{
	[System.Serializable]
	public class UnitHatData
	{
	    [SerializeField] private int _id;
	    [SerializeField] private Sprite _selectSprite;
	    [SerializeField] private Sprite _previewSprite;
		[SerializeField] private Sprite _previewOverlaySprite;
		[SerializeField] private Hat _prefab;
	    [SerializeField] private int _requredLevel;

	    public int Id => _id;
	    public Sprite SelectSprite => _selectSprite;
	    public Sprite PreviewSprite => _previewSprite;
		public Sprite PreviewOverlaySprite => _previewOverlaySprite;
		public Hat Prefab => _prefab;
	    public int RequredLevel => _requredLevel;
	}
}
