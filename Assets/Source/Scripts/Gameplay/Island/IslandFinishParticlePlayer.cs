using SlimeGround.Gameplay.Levels;
using UnityEngine;

namespace SlimeGround.Gameplay.Islands
{
	public class IslandFinishParticlePlayer
	{
	    private LevelProgressTracker _levelProgressTracker;
	    private IslandFinishParticlePool _particlePool;

	    public IslandFinishParticlePlayer(LevelProgressTracker levelProgressTracker, IslandFinishParticlePool particlePool)
	    {
	        _levelProgressTracker = levelProgressTracker;
	        _particlePool = particlePool;

	        _levelProgressTracker.IslandFinished += OnIslandFinished;
	    }

	    private void OnIslandFinished(Island island)
	    {
	        ParticleSystem particle = _particlePool.GetFreeParticle();
	        particle.transform.position = island.CenterPoint.position;
	    }
	}
}
