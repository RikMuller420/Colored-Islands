using UnityEngine;

namespace SlimeGround.Effects
{
	public class SkyCloudRotator : MonoBehaviour
	{
		private const string RotationName = "_Rotation";

		[SerializeField] private Material _skyboxMaterial;
		[SerializeField] private float _rotationSpeed = 1f;

		private float _rotation = 0f;

		private void Update()
		{
			_rotation += _rotationSpeed * Time.deltaTime;
			_skyboxMaterial.SetFloat(RotationName, _rotation);
		}
	}
}
