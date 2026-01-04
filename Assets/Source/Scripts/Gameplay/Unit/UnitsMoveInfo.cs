using System.Collections.Generic;

public class UnitsMoveInfo
{
    public UnitsMoveInfo(BaseIsland startIsland, BaseIsland endIsland,
                         Paint unitsPaint, IReadOnlyCollection<Unit> units)
    {
        StartIsland = startIsland;
        EndIsland = endIsland;
        UnitsPaint = unitsPaint;
        Units = units;
    }

    public BaseIsland StartIsland { get; }
    public BaseIsland EndIsland { get; }
    public Paint UnitsPaint { get; }
    public IReadOnlyCollection<Unit> Units { get; }
}
