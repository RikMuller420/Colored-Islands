using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Island : BaseIsland
{
    [SerializeField] private Transform _centerPoint;

    private IslandRenderer _renderer;
    private IReadOnlyCollection<SpriteRenderer> _points;

    public event Action<Island> IslandFinished;

    public Paint Paint { get; private set; }
    public bool IsDone => Points.All(point => !point.IsFree && point.OccupiedUnit.Paint == Paint);
    public Transform CenterPoint => _centerPoint;

    private void OnEnable()
    {
        UnitAdded += TryFinish;
    }

    private void OnDisable()
    {
        UnitAdded -= TryFinish;
    }

    public void Initialize(List<IslandPoint> placementPoints, Paint paint, PaintMaterials paintMaterials)
    {
        base.Initialize(placementPoints);

        _points = Points.Select(point => point.Point).ToList().AsReadOnly();
        _renderer = new IslandRenderer(MeshRenderer, paintMaterials);
        SetPaint(paint);
    }

    public void SetPaint(Paint paint)
    {
        Paint = paint;
        _renderer.SetPaint(paint, _points);
    }

    public void TryFinish()
    {
        if (IsDone == false)
        {
            return;
        }

        foreach (IslandPoint point in Points)
        {
            point.OccupiedUnit.Animator.Jump();
        }

        Deactivate();
        IslandFinished?.Invoke(this);
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

#if UNITY_EDITOR
    public void SetCenterPoint(Transform centerPoint)
    {
        _centerPoint = centerPoint;
    }
#endif
}
