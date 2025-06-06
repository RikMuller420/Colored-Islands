using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandPaintDistributor
{
    public Dictionary<IslandPoint, Paint> CalculateOptimalPaintDistribution(BaseIsland island)
    {
        List<IslandPoint> occupiedPoints = GetOccupiedPoints(island.Points);
        Dictionary<Paint, int> unitsCountByPaints = CountUnitsByPaint(occupiedPoints);
        List<Paint> sortedPaints = GetSortedPaints(occupiedPoints);

        return DistributePoints(island.Points.ToList(), sortedPaints, unitsCountByPaints, island);
    }

    private Dictionary<IslandPoint, Paint> DistributePoints(List<IslandPoint> sortedPoints, List<Paint> sortedPaints,
                                                            Dictionary<Paint, int> unitsCountByPaints, BaseIsland island)
    {
        Dictionary<IslandPoint, Paint> requredPointPaints = new Dictionary<IslandPoint, Paint>();
        int pointIndex = 0;

        Dictionary<IslandPoint, Paint> usedPoints = new Dictionary<IslandPoint, Paint>();
        List<IslandPoint> aviablePoints = island.Points.ToList();

        foreach (Paint paint in sortedPaints)
        {
            int unitsCount = unitsCountByPaints[paint];

            for (int i = 0; i < unitsCount; i++)
            {
                IslandPoint bestPoint = FindBestPoint(paint, aviablePoints, usedPoints);

                requredPointPaints.Add(bestPoint, paint);
                aviablePoints.Remove(bestPoint);
                usedPoints.Add(bestPoint, paint);

                pointIndex++;
            }
        }

        return requredPointPaints;
    }

    public IslandPoint FindBestPoint(Paint paint, List<IslandPoint> aviablePoints, Dictionary<IslandPoint, Paint> usedPoints)
    {
        KeyValuePair<IslandPoint, Paint> startPointPaint = usedPoints.FirstOrDefault(point => point.Value == paint);

        if (startPointPaint.Key != null)
        {
            IslandPoint closestPoint = ClosestPoint(startPointPaint.Key.Point.position, aviablePoints);

            return closestPoint;
        }

        return aviablePoints.FirstOrDefault();
    }

    public static IslandPoint ClosestPoint(Vector3 startPoint, List<IslandPoint> points)
    {
        return points.OrderBy(point => Math.Pow(point.Point.position.x - startPoint.x, 2) +
                                       Math.Pow(point.Point.position.z - startPoint.z, 2))
                     .FirstOrDefault();
    }

    private List<Paint> GetSortedPaints(List<IslandPoint> sortedPoints)
    {
        List<Paint> sortedPaints = new List<Paint>();

        foreach (IslandPoint point in sortedPoints)
        {
            if (sortedPaints.Contains(point.OccupiedUnit.Paint))
            {
                continue;
            }

            sortedPaints.Add(point.OccupiedUnit.Paint);
        }

        return sortedPaints;
    }

    private List<IslandPoint> GetOccupiedPoints(IReadOnlyCollection<IslandPoint> sortedPoints)
    {
        List<IslandPoint> occupiedPoints = new List<IslandPoint>();

        foreach (IslandPoint point in sortedPoints)
        {
            if (point.IsFree == false)
            {
                occupiedPoints.Add(point);
            }
        }

        return occupiedPoints;
    }

    private Dictionary<Paint, int> CountUnitsByPaint(List<IslandPoint> occupiedPoints)
    {
        Dictionary<Paint, int> unitsCountByPaint = new Dictionary<Paint, int>();

        foreach (IslandPoint occupiedPoint in occupiedPoints)
        {
            Paint paint = occupiedPoint.OccupiedUnit.Paint;

            if (unitsCountByPaint.ContainsKey(paint))
            {
                unitsCountByPaint[paint]++;
            }
            else
            {
                unitsCountByPaint.Add(paint, 1);
            }
        }

        return unitsCountByPaint;
    }
}
