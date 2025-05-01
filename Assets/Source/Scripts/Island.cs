using System;
using System.Collections.Generic;
using UnityEngine;

public class Island : BaseIsland
{
    private IslandRenderer _renderer;

    public event Action IslandFinished;

    public Paint Paint { get; private set; }

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
        foreach (PlacementPoint point in Points)
        {
            if (point.IsFree)
            {
                return;
            }

            if (point.OccupiedUnit.Paint != Paint)
            {
                return;
            }
        }

        BlockIsland();
        IslandFinished?.Invoke();
    }

    private void BlockIsland()
    {
        enabled = false;
        //Выключить меш коллайдер
        //выключить меш коллайдреы на юнитах
    }
}
