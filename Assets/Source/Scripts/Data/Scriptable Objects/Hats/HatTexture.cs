using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.Hats
{
	[System.Serializable]
	public class HatTexture
    {
		[SerializeField] private HatTextureType _type;
		[SerializeField] private Texture _texture;

		public HatTextureType Type => _type;
		public Texture Texture => _texture;
	}
}
