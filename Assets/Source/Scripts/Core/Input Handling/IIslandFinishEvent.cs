using System;

public interface IIslandFinishEvent
{
    public event Action<Island> IslandFinished;
}
