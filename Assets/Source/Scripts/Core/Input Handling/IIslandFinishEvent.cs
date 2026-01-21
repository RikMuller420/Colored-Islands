using System;
using SlimeGround.Gameplay.Islands;

namespace SlimeGround.Core.InputHandling
{
	public interface IIslandFinishEvent
	{
	    public event Action<Island> IslandFinished;
	}
}
