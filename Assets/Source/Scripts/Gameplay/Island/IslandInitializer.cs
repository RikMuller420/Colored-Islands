using System;
using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data;
using SlimeGround.Data.ScriptableObjects.Paints;
using SlimeGround.Gameplay.Units;
using SlimeGround.Menu.Windows.Customization;
using UnityEngine;

namespace SlimeGround.Gameplay.Islands
{
	[RequireComponent(typeof(Island))]
	public class IslandInitializer : MonoBehaviour
	{
	    [SerializeField] private UnitSlotType _unitSlot;
	    [SerializeField] private Island _island;
	    [SerializeField] private Transform _rootOfPoints;
	    [SerializeField] private List<SpriteRenderer> _points = new List<SpriteRenderer>();
	    [SerializeField] private List<IslandStartUnits> _startUnits = new List<IslandStartUnits>();

	    public int PointsCount => _points.Count;
	    public UnitSlotType UnitSlot => _unitSlot;
	    public Island Island => _island;
	    public Transform RootOfPoints => _rootOfPoints;
	    public IReadOnlyList<SpriteRenderer> Points => new List<SpriteRenderer>(_points);
	    public List<IslandStartUnits> StartUnits => new List<IslandStartUnits>(_startUnits);

	    public void Initialize(Func<Unit> createUnit, ColorSampleMaterials paintMaterials, Transform unitsLookAtPoint,
	                          CustomizationSettingsHolder customizationSettings, ColorSample colorSample, float unitScale)
	    {
	        FindRequireComponents();
	        List<IslandPoint> placementPoints = new List<IslandPoint>();

	        foreach(SpriteRenderer point in _points)
	        {
	            placementPoints.Add(new IslandPoint(point));            
	        }

	        _island.Initialize(placementPoints, UnitSlot, colorSample, paintMaterials);

	        foreach (IslandStartUnits startUnits in _startUnits)
	        {
	            for (int i = 0; i < startUnits.Amout; i++)
	            {
	                Unit unit = createUnit.Invoke();
	                unit.Initialize(customizationSettings);
	                unit.SetScale(unitScale);
	                unit.ResetRotation();
	                unit.SetIsland(_island);
	                unit.SetUnitSlot(startUnits.Slot);
	                unit.Activate();

	                _island.AddStartUnit(unit, out IslandPoint placementPoint);
	                unit.transform.position = placementPoint.Transform.position;
	                unit.MeshTransform.LookAt(unitsLookAtPoint);
	            }
	        }
	    }

	    public void SetStartUnits(List<IslandStartUnits> startUnits)
	    {
	        _startUnits = startUnits;
	    }

	    public void FindRequireComponents()
	    {
	        _island = GetComponent<Island>();
	    }

	    public void SetRequredUnitSlot(UnitSlotType unitSlot)
	    {
	        _unitSlot = unitSlot;
	    }

	    public void FillPoints(Transform rootOfPoints)
	    {
	        _rootOfPoints = rootOfPoints;
	        _points.Clear();
	        _points.AddRange(_rootOfPoints.GetComponentsInChildren<SpriteRenderer>()
	                .Where(transform => transform != RootOfPoints));
	    }

	    [ContextMenu("Fill Points")]
	    public void FillPoints()
	    {
	        FillPoints(_rootOfPoints);
	    }
	}
}
