using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BuferIslandsHolder : MonoBehaviour
{
    private LevelSettings _levelSettings;
    private UnitMover _unitMover;
    private Vector3 _hidedPostion = new Vector3(0, -0.5f, -5.3f);
    private Vector3 _defaultPostion = new Vector3(0, 0, -5.3f);
    private float _appearDuration = 0.3f;

    public BaseIsland CurrentIsland { get; private set; }

    public void Initialize(LevelSettings levelSettings, UnitMover unitMover)
    {
        _levelSettings = levelSettings;
        _unitMover = unitMover;
    }

    public void LoadIsland(int size)
    {
        BaseIsland oldIsland = CurrentIsland;
        DeactivateCurrentIsland();

        BuferIslandInitializer islandInitializer = Instantiate(GetIslandPrefab(size));
        islandInitializer.ResetPoints();
        CurrentIsland = islandInitializer.Island;
        CurrentIsland.Collider.enabled = false;
        CurrentIsland.gameObject.SetActive(true);

        islandInitializer.transform.position = _hidedPostion;
        islandInitializer.transform.DOMove(_defaultPostion, _appearDuration)
                                   .SetEase(Ease.InOutQuad)
                                   .OnComplete(() =>
                                   {
                                       CurrentIsland.Collider.enabled = true;
                                       SwapUnitsToNewIsland(oldIsland);
                                   });
    }

    public void DeactivateCurrentIsland()
    {
        if (CurrentIsland != null)
        {
            Destroy(CurrentIsland.gameObject);
        }

        CurrentIsland = null;
    }

    public void SwapToNewIsland(int size)
    {
        foreach (IslandPoint point in CurrentIsland.Points)
        {
            if (point.IsFree == false)
            {
                point.OccupiedUnit.Animator.Jump();
            }
        }

        CurrentIsland.transform.DOMove(_hidedPostion, _appearDuration)
                               .SetEase(Ease.InOutQuad)
                               .OnComplete(() => 
                                {
                                    LoadIsland(size);
                                });
    }

    private void SwapUnitsToNewIsland(BaseIsland oldIsland)
    {
        if (oldIsland == null)
        {
            return;
        }

        foreach (IslandPoint point in oldIsland.Points)
        {
            if (point.IsFree == false)
            {
                _unitMover.MoveUnit(point.OccupiedUnit, CurrentIsland);
            }
        }
    }

    private BuferIslandInitializer GetIslandPrefab(int size)
    {
        return _levelSettings.BuferIslands.FirstOrDefault(island => island.Size == size);
    }
}
