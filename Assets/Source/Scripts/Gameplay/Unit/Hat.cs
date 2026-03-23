using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	public class Hat : MonoBehaviour
	{
		[SerializeField] private HatTextureType _textureType;
	    [SerializeField] private GameObject _gameObject;
	    [SerializeField] private MeshRenderer _meshRenderer;

		public HatTextureType TextureType => _textureType; 
		public GameObject GameObject => _gameObject;
	    public MeshRenderer MeshRenderer => _meshRenderer;
	}
}
