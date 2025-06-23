using UnityEngine;

public class TrainigSequenceLevel1 : MonoBehaviour
{
    [SerializeField] private Island _firstMoveIsland;
    [SerializeField] private Paint _firstMovePaint;

    private LevelObjectsHolder _levelObjectsHolder;
    private BuferIslandsHolder _buferIslandsHolder;
    private SelectHandler _selectHandler;

    public void Initialize(LevelObjectsHolder levelObjectsHolder, BuferIslandsHolder buferIslandsHolder, SelectHandler selectHandler)
    {
        _levelObjectsHolder = levelObjectsHolder;
        _buferIslandsHolder = buferIslandsHolder;
        _selectHandler = selectHandler;
    }

    public void StartTraining()
    {
        ActivateFirstMoveHint();
        _selectHandler.UnitsSelected += OnUnitsSelected;
    }

    private void ActivateFirstMoveHint()
    {
        DeactivateAllColliders();
        ActivateUnitsColliders(_firstMoveIsland, _firstMovePaint);
    }

    private void OnUnitsSelected()
    {
        
    }

    private void ActivateUnitsColliders(Island island, Paint unitsPaint)
    {
        foreach (IslandPoint islandPoint in island.Points)
        {
            if (islandPoint.IsFree == false && islandPoint.OccupiedUnit.Paint == unitsPaint)
            {
                islandPoint.OccupiedUnit.Collider.enabled = true;
            }
        }
    }

    private void DeactivateAllColliders()
    {
        foreach (Island island in _levelObjectsHolder.Islands)
        {
            DeactivateColliders(island);
        }

        DeactivateColliders(_buferIslandsHolder.CurrentIsland);
    }

    private void DeactivateColliders(BaseIsland island)
    {
        island.Collider.enabled = false;

        foreach (IslandPoint islandPoint in island.Points)
        {
            if (islandPoint.IsFree)
            {
                continue;
            }

            islandPoint.OccupiedUnit.Collider.enabled = false;
        }
    }
}
