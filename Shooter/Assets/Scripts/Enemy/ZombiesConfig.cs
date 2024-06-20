using System;
using NaughtyAttributes;
using UnityEngine;

namespace Shooter.Enemy
{
    [Serializable]
    public class Zombie
    {
        [SerializeField] public Enemy zombie;
        [SerializeField, Range(1, 100)] public int probability;
    }

    [Serializable]
    public class DifficultyMode
    {
        [SerializeField, Min(1)] public int baseEnemyCount;
        [SerializeField] public float enemyCountIncreaseRate;
        [SerializeField] public float waveDelay;
        [SerializeField] public Zombie[] zombies;
    }
    
    [CreateAssetMenu(fileName = "Preset", menuName = "DifficultyConfig/Preset", order = 1)]
    [Serializable]
    public class ZombiesConfig : ScriptableObject
    {
        [SerializeField] public DifficultyMode easy;
        
        [SerializeField] public DifficultyMode normal;
        
        [SerializeField] public DifficultyMode hard;
        
        [SerializeField] public DifficultyMode nightmare;
    }
}