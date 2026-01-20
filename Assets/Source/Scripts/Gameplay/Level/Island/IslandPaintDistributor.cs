using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandPaintDistributor
{
    public Dictionary<IslandPoint, UnitSlotType> CalculateOptimalSlotDistribution(BaseIsland island)
    {
        List<IslandPoint> occupiedPoints = GetOccupiedPoints(island.Points);
        Dictionary<UnitSlotType, int> unitsCountBySlot = CountUnitsBySlot(occupiedPoints);
        List<UnitSlotType> sortedPaints = GetSortedUnitSlots(occupiedPoints);

        return DistributePoints(island.Points.ToList(), sortedPaints, unitsCountBySlot, island);
    }

    private Dictionary<IslandPoint, UnitSlotType> DistributePoints(List<IslandPoint> sortedPoints, List<UnitSlotType> sortedSlots,
                                                            Dictionary<UnitSlotType, int> unitsCountBySlot, BaseIsland island)
    {
        Dictionary<IslandPoint, UnitSlotType> requredPointSlots = new Dictionary<IslandPoint, UnitSlotType>();
        int pointIndex = 0;

        Dictionary<IslandPoint, UnitSlotType> usedPoints = new Dictionary<IslandPoint, UnitSlotType>();
        List<IslandPoint> aviablePoints = island.Points.ToList();

        foreach (UnitSlotType unitSlot in sortedSlots)
        {
            int unitsCount = unitsCountBySlot[unitSlot];

            for (int i = 0; i < unitsCount; i++)
            {
                IslandPoint bestPoint = FindBestPoint(unitSlot, aviablePoints, usedPoints);

                requredPointSlots.Add(bestPoint, unitSlot);
                aviablePoints.Remove(bestPoint);
                usedPoints.Add(bestPoint, unitSlot);

                pointIndex++;
            }
        }

        return requredPointSlots;
    }

    public IslandPoint FindBestPoint(UnitSlotType slot, List<IslandPoint> aviablePoints, Dictionary<IslandPoint, UnitSlotType> usedPoints)
    {
        KeyValuePair<IslandPoint, UnitSlotType> startPointSlots = usedPoints.FirstOrDefault(point => point.Value == slot);

        if (startPointSlots.Key != null)
        {
            IslandPoint closestPoint = ClosestPoint(startPointSlots.Key.Transform.position, aviablePoints);

            return closestPoint;
        }

        return aviablePoints.FirstOrDefault();
    }

    public static IslandPoint ClosestPoint(Vector3 startPoint, List<IslandPoint> points)
    {
        return points.OrderBy(point => Math.Pow(point.Transform.position.x - startPoint.x, 2) +
                                       Math.Pow(point.Transform.position.z - startPoint.z, 2))
                     .FirstOrDefault();
    }

    private List<UnitSlotType> GetSortedUnitSlots(List<IslandPoint> sortedPoints)
    {
        List<UnitSlotType> sortedSlots = new List<UnitSlotType>();

        foreach (IslandPoint point in sortedPoints)
        {
            if (sortedSlots.Contains(point.OccupiedUnit.Slot))
            {
                continue;
            }

            sortedSlots.Add(point.OccupiedUnit.Slot);
        }

        return sortedSlots;
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

    private Dictionary<UnitSlotType, int> CountUnitsBySlot(List<IslandPoint> occupiedPoints)
    {
        Dictionary<UnitSlotType, int> unitsCountBySlots = new Dictionary<UnitSlotType, int>();

        foreach (IslandPoint occupiedPoint in occupiedPoints)
        {
            UnitSlotType slot = occupiedPoint.OccupiedUnit.Slot;

            if (unitsCountBySlots.ContainsKey(slot))
            {
                unitsCountBySlots[slot]++;
            }
            else
            {
                unitsCountBySlots.Add(slot, 1);
            }
        }

        return unitsCountBySlots;
    }
}
