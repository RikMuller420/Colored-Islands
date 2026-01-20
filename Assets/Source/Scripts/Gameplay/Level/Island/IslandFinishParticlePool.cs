using System.Collections.Generic;
using UnityEngine;

public class IslandFinishParticlePool : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particlePrefab;

    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    public ParticleSystem GetFreeParticle()
    {
        foreach (ParticleSystem particle in _particles)
        {
            if (particle.gameObject.activeSelf == false)
            {
                particle.gameObject.SetActive(true);

                return particle;
            }
        }

        ParticleSystem newParticle = Instantiate(_particlePrefab, transform);
        _particles.Add(newParticle);

        return newParticle;
    }
}
