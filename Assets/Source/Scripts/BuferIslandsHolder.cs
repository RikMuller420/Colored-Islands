using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuferIslandsHolder : MonoBehaviour
{
    [SerializeField] private List<BuferIslandInitializer> _buferIslands = new List<BuferIslandInitializer>();

    public BuferIslandInitializer GetIsland(int size)
    {
        return _buferIslands.FirstOrDefault(island => island.Size == size);
    }
}
