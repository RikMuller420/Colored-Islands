using System;
using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data;
using SlimeGround.Data.ScriptableObjects.Paints;
using UnityEngine;

namespace SlimeGround.Gameplay.Islands
{
	[RequireComponent(typeof(MeshRenderer))]
	public class Island : BaseIsland
	{
	    [SerializeField] private Transform _centerPoint;

	    private IslandRenderer _renderer;
	    private IReadOnlyCollection<SpriteRenderer> _points;

	    public event Action<Island> IslandFinished;

	    public UnitSlotType RequredUnitSlot { get; private set; }
	    public bool IsDone => Points.All(point => !point.IsFree && point.OccupiedUnit.Slot == RequredUnitSlot);
	    public Transform CenterPoint => _centerPoint;

	    private void OnEnable()
	    {
	        UnitAdded += TryFinish;
	    }

	    private void OnDisable()
	    {
	        UnitAdded -= TryFinish;
	    }

	    public void Initialize(List<IslandPoint> placementPoints, UnitSlotType unitSlot,
							ColorSample colorSample, ColorSampleMaterials paintMaterials)
	    {
	        Initialize(placementPoints);

	        _points = Points.Select(point => point.Point).ToList().AsReadOnly();
	        _renderer = new IslandRenderer(MeshRenderer, paintMaterials);

	        SetRequredUnitSlot(unitSlot, colorSample);
	    }

	    public void SetRequredUnitSlot(UnitSlotType unitSlot, ColorSample colorSample)
	    {
	        RequredUnitSlot = unitSlot;
	        _renderer.SetPaint(colorSample, _points);
	    }

	    public void TryFinish()
	    {
	        if (IsDone == false)
	        {
	            return;
	        }

	        foreach (IslandPoint point in Points)
	        {
	            point.OccupiedUnit.Animator.TryJump();
	        }

	        Deactivate();
	        IslandFinished?.Invoke(this);
	    }

	    public void Deactivate()
	    {
	        enabled = false;
	        GetComponent<Collider>().enabled = false;

	        foreach (IslandPoint point in Points)
	        {
	            if (point.IsFree == false)
	            {
	                point.OccupiedUnit.Deactivate();
				}
			}
	    }

	    public void Activate()
	    {
	        enabled = true;
	        GetComponent<Collider>().enabled = true;

	        foreach (IslandPoint point in Points)
	        {
	            if (point.IsFree == false)
	            {
	                point.OccupiedUnit.Activate();
				}
			}
	    }

	#if UNITY_EDITOR
	    public void SetCenterPoint(Transform centerPoint)
	    {
	        _centerPoint = centerPoint;
	    }
	#endif
	}
}
