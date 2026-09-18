using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SlimeGround.Effects.Particles
{
	public class ParticlePool : MonoBehaviour
	{
		[SerializeField] private ParticleSystem _prefab;

		private List<GameObject> _particles = new();

		public GameObject GetParticle()
		{
			GameObject freeParticle = _particles.FirstOrDefault(ripple => ripple.activeSelf == false);

			if (freeParticle == null)
			{
				freeParticle = Instantiate(_prefab.gameObject, transform);
				_particles.Add(freeParticle);
			}

			return freeParticle;
		}
	}
}
