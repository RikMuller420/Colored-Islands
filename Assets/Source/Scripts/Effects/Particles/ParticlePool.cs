using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SlimeGround.Effects.Particles
{
	public class ParticlePool : MonoBehaviour
	{
		[SerializeField] private ParticleSystem _prefab;

		private List<GameObject> _collection = new();

		public GameObject GetParticle()
		{
			GameObject freeRipple = _collection.FirstOrDefault(ripple => ripple.activeSelf == false);

			if (freeRipple == null)
			{
				freeRipple = Instantiate(_prefab.gameObject, transform);
				_collection.Add(freeRipple);
			}

			return freeRipple;
		}
	}
}
