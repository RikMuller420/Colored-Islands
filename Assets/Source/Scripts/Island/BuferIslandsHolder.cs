using System.Linq;
using UnityEngine;

public class BuferIslandsHolder : MonoBehaviour
{
    private LevelSettings _levelSettings;

    public BaseIsland CurrentIsland { get; private set; }

    public void Initialize(LevelSettings levelSettings)
    {
        _levelSettings = levelSettings;
    }

    public void LoadIsland(int size)
    {
        BuferIslandInitializer islandInitializer = Instantiate(GetIslandPrefab(size));
        islandInitializer.ResetPoints();
        CurrentIsland = islandInitializer.Island;
        CurrentIsland.gameObject.SetActive(true);
    }

    public void DeactivateCurrentIsland()
    {
        if (CurrentIsland != null)
        {
            Destroy(CurrentIsland.gameObject);
        }

        CurrentIsland = null;
    }

    private BuferIslandInitializer GetIslandPrefab(int size)
    {
        return _levelSettings.BuferIslands.FirstOrDefault(island => island.Size == size);
    }
}
