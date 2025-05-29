using System;
using UnityEngine;

public class AngryBar : MonoBehaviour
{
    public event Action Changed;

    public float Value;

    public void SetValue(float value)
    {
        Value = Mathf.Clamp01(value);
        Changed?.Invoke();
    }
}
