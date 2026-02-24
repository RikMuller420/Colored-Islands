using SlimeGround.Effects.Particles;
using UnityEngine;

namespace SlimeGround.Core.InputHandling
{
	public class MenuClickBehaviour : ClickBehaviour
	{
		private ParticlePool _splashPool;
		private ParticlePool _leavesHitPool;

		public MenuClickBehaviour(ParticlePool splashPool, ParticlePool leavesHitPool,
								  LayerMask layerMask) : base(layerMask)
		{
			_splashPool = splashPool;
			_leavesHitPool = leavesHitPool;
		}

		public override void HandleClick(RaycastHit hit)
		{
			if (hit.collider.TryGetComponent(out IMenuClickable clickable))
			{
				Select(clickable, hit);
			}
		}

		public override void ResetBehaviour() { return; }

		private void Select(IMenuClickable clickable, RaycastHit hit)
		{
			switch (clickable)
			{
				case WaterRippleZone rippleZone:
					PlayParticle(_splashPool, hit);
					break;

				case LeavesHitZone leavesHit:
					PlayParticle(_leavesHitPool, hit);
					break;
			}
		}

		private void PlayParticle(ParticlePool pool, RaycastHit hit)
		{
			GameObject particle = pool.GetParticle();
			particle.transform.position = hit.point;
			particle.gameObject.SetActive(true);
		}
	}
}