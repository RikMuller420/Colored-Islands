using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class Island : BaseIsland
{
    private IslandRenderer _renderer;
    private float _instability;
    private float _instabilityLimit = 500;

    public event Action IslandFinished;

    public float Instablility => _instability / _instabilityLimit;  
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

    private void Update()
    {
        float instabilityStep = 0f;

        foreach (IslandPoint point in Points)
        {
            if (point.IsFree == false && point.OccupiedUnit.Paint != Paint)
            {
                instabilityStep += 1f;
            }
        }

        instabilityStep *= 1f;
        _instability += instabilityStep * Time.deltaTime;
        Debug.Log(gameObject.name + "  " + Instablility);
    }

    public void Initialize(List<IslandPoint> placementPoints, Paint paint, PaintMaterials paintMaterials)
    {
        base.Initialize(placementPoints);

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        _renderer = new IslandRenderer(renderer, paintMaterials);
        SetPaint(paint);
        _instability = 0f;
    }

    public void SetPaint(Paint paint)
    {
        Paint = paint;
        _renderer.SetPaint(paint);
    }

    public void TryFinish()
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

        foreach (IslandPoint point in Points)
        {
            point.OccupiedUnit.Deactivate();
        }
    }
}
