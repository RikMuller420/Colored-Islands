using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Gameplay.Islands
{
	public class BuferIslandsHolder : MonoBehaviour
	{
	    [SerializeField] private LevelSettings _levelSettings;

	    private UnitMover _unitMover;
	    private Vector3 _hidedPostion = new Vector3(0, -0.5f, -5.3f);
	    private Vector3 _defaultPostion = new Vector3(0, 0, -5.3f);
	    private float _appearDuration = 0.3f;

	    public BaseIsland CurrentIsland { get; private set; }

	    public void Initialize(UnitMover unitMover)
	    {
	        _unitMover = unitMover;
	    }

	    public void LoadIsland(int size)
	    {
	        List<Unit> currentIslandUnits = GetCurrentIslandUnits();
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
	                                       SwapUnitsToNewIsland(currentIslandUnits);
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

	    private List<Unit> GetCurrentIslandUnits()
	    {
	        if (CurrentIsland == null)
	        {
	            return new List<Unit>();
	        }

	        return CurrentIsland.Points.Where(point => point.IsFree == false)
	                                   .Select(point => point.OccupiedUnit)
	                                   .ToList();
	    }

	    private void SwapUnitsToNewIsland(List<Unit> units)
	    {
	        foreach (Unit unit in units)
	        {
	            _unitMover.MoveUnit(unit, CurrentIsland);
	        }
	    }

	    private BuferIslandInitializer GetIslandPrefab(int size)
	    {
	        return _levelSettings.BuferIslands.FirstOrDefault(island => island.Size == size);
	    }
	}
}
