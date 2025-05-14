using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuferIslandsHolder : MonoBehaviour
{
    [SerializeField] private List<BuferIslandInitializer> _buferIslands = new List<BuferIslandInitializer>();

    public BaseIsland CurrentIsland { get; private set; }

    public void LoadIsland(int size)
    {
        BuferIslandInitializer islandInitializer = GetIsland(size);
        islandInitializer.ResetPoints();
        CurrentIsland = islandInitializer.Island;
        CurrentIsland.gameObject.SetActive(true);
    }

    public void DeactivateCurrentIsland()
    {
        if (CurrentIsland != null)
        {
            CurrentIsland.gameObject.SetActive(false);
        }

        CurrentIsland = null;
    }

    private BuferIslandInitializer GetIsland(int size)
    {
        return _buferIslands.FirstOrDefault(island => island.Size == size);
    }
}
