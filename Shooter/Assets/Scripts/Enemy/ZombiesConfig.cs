using System;
using UnityEngine;

namespace Shooter.Enemy
{
    [Serializable]
    public class Zombie
    {
        [SerializeField] public Enemy zombie;
        [SerializeField, Range(1, 100)] public int probability;
    }
    
    [CreateAssetMenu(fileName = "Preset", menuName = "DifficultyConfig/Preset", order = 1)]
    [Serializable]
    public class ZombiesConfig : ScriptableObject
    {
        [Tooltip("Keep sum probability = 100")]
        [SerializeField] public Zombie[] easyZombies;
        
        [Tooltip("Keep sum probability = 100")] 
        [SerializeField] public Zombie[] normalZombies;
        
        [Tooltip("Keep sum probability = 100")]
        [SerializeField] public Zombie[] hardZombies;
        
        [Tooltip("Keep sum probability = 100")]
        [SerializeField] public Zombie[] nightmareZombies;
    }
}