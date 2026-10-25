using System;
using UnityEngine;

[Serializable]
public class HapticPulse
{
    [Range(0f, 1f)] public float Amplitude;
    public float Duration;

    public HapticPulse(float amplitude, float duration)
    {
        Amplitude = amplitude;
        Duration = duration;
    }
}
