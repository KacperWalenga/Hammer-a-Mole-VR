using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class DifficultyLevel
{
    [field:SerializeField] public float RaiseTime { get; private set; } = 2f;
    [field:SerializeField] public float MinRaiseTime { get; private set; } = 0.75f;
    [field:SerializeField] public float RaiseTimeDecreasePerSecond { get; private set; } = 0.01f;
    [field:SerializeField] public float ScoreMultiplier { get; private set; } = 1f;
    [field:SerializeField] public int MaxHealth { get; private set; } = 3;

    [Header("Special moles chances")]
    [field:SerializeField, Range(0f, 1f)] public float GoldenChance { get; private set; } = 0.1f;
    [field:SerializeField, Range(0f, 1f)] public float BombChance { get; private set; } = 0.05f;
    [field:SerializeField, Range(0f, 1f)] public float HeartChance { get; private set; } = 0.08f;

    public DifficultyLevel(float raiseTime, float minRaiseTime, float raiseTimeDecreasePerSecond, float scoreMultiplier, 
        int maxHealth, float goldenChance, float bombChance, float heartChance)
    {
        RaiseTime = raiseTime;
        MinRaiseTime = minRaiseTime;
        RaiseTimeDecreasePerSecond = raiseTimeDecreasePerSecond;
        ScoreMultiplier = scoreMultiplier;
        MaxHealth = maxHealth;
        GoldenChance = goldenChance;
        BombChance = bombChance;
        HeartChance = heartChance;
    }

    public MoleType RollMoleType()
    {
        var roll = Random.value;

        if (roll < GoldenChance)
            return MoleType.Golden;

        roll -= GoldenChance;

        if (roll < BombChance)
            return MoleType.Bomb;

        roll -= BombChance;

        return roll < HeartChance ? MoleType.Heart : MoleType.Normal;
    }

    public float GetRaiseTime(float elapsedTime)
    {
        return Mathf.Max(MinRaiseTime, RaiseTime - RaiseTimeDecreasePerSecond * elapsedTime);
    }
}
