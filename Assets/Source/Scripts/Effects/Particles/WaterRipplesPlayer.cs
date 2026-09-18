using System.Collections;
using UnityEngine;

namespace SlimeGround.Effects.Particles
{
	public class WaterRipplesPlayer : MonoBehaviour
	{
		private const float MinIntervalTime = 3.5f;
		private const float MaxIntervalTime = 7f;

		[SerializeField] private ParticlePool _ripplePool;
		[SerializeField] private MeshFilter _rippleZone;

		private void Start()
		{
			StartCoroutine(PlayRipples());
		}

		private IEnumerator PlayRipples()
		{
			while (enabled)
			{
				float awaitTime = Random.Range(MinIntervalTime, MaxIntervalTime);

				yield return new WaitForSeconds(awaitTime);

				PlayRipple();
			}
		}

		private void PlayRipple()
		{
			GameObject ripple = _ripplePool.GetParticle();
			ripple.transform.position = GetRandomRipplePosition();
			ripple.SetActive(true);
		}

		private Vector3 GetRandomRipplePosition()
		{
			Bounds bounds = _rippleZone.sharedMesh.bounds;

			float minX = bounds.min.x;
			float maxX = bounds.max.x;
			float minZ = bounds.min.z;
			float maxZ = bounds.max.z;

			Vector3 localPoint = new Vector3
			(
				Random.Range(minX, maxX),
				_rippleZone.transform.position.y,
				Random.Range(minZ, maxZ)
			);

			return _rippleZone.transform.TransformPoint(localPoint);
		}
	}
}
