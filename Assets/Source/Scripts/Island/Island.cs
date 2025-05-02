using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Island : BaseIsland
{
    private IslandRenderer _renderer;

    public event Action IslandFinished;

    public Paint Paint { get; private set; }
    public bool IsDone => Points.All(point => !point.IsFree && point.OccupiedUnit.Paint == Paint);

    private void OnEnable()
    {
        UnitAdded += TryFinish;
    }

    private void OnDisable()
    {
        UnitAdded -= TryFinish;
    }

    public void Initialize(List<PlacementPoint> placementPoints, Paint paint, PaintMaterials paintMaterials)
    {
        base.Initialize(placementPoints);

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        _renderer = new IslandRenderer(renderer, paintMaterials);
        SetPaint(paint);
    }

    private void SetPaint(Paint paint)
    {
        Paint = paint;
        _renderer.SetPaint(paint);
    }

    private void TryFinish()
    {
        if (IsDone == false)
        {
            return;
        }

        Deactivate();
        IslandFinished?.Invoke();
    }

    private void Deactivate()
    {
        enabled = false;
        GetComponent<Collider>().enabled = false;

        foreach (PlacementPoint point in Points)
        {
            point.OccupiedUnit.Deactivate();
        }
    }
}
