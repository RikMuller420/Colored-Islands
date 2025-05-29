using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryTracker
{
    private float _instability;
    private float _instabilityLimit = 500;
    public float Instablility => _instability / _instabilityLimit;


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
}
