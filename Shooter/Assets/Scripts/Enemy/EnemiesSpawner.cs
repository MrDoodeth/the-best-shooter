using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
namespace Shooter.Enemy
{
    public enum Difficulty { Easy, Normal, Hard, Nightmare }
    
    public class EnemiesSpawner : MonoBehaviour
    {
        [Header("Wave Settings")]
        [SerializeField][Min(1)] private int baseEnemyCount = 3;
        [SerializeField] private float enemyCountIncreaseRate = 1.2f;
        [SerializeField] private float waveDelay;
        [SerializeField] private Transform[] spawnPoints;
        [Space(15)]
        
        [SerializeField][Range(1, 10)] private int maxExplosiveEnemyAmount;
        [Space(15)]
        
        [SerializeField] private Difficulty difficulty;
        [SerializeField] public ZombiesConfig zombiesConfig;
        private Zombie[] currentZombies;
        private int currentExplosiveEnemyAmount;

        public static Action OnStartNewWave;
        public static Action OnEndWave;
        public static Action OnEnemyDeath;

        private static int currentEnemyCount;
        private int currentWaveNumber = 1;
        private int selectedSpawnerIndex = 0;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;      
            for (int i = 0; i < spawnPoints.Length && spawnPoints != null; i++)
                Gizmos.DrawWireCube(spawnPoints[i].position, Vector3.one / 2);
        }
        private void Awake()
        {
            #region InitDifficulty
            switch (difficulty)
            {
                case Difficulty.Easy:
                    currentZombies = zombiesConfig.easyZombies; break;
            
                case Difficulty.Normal:
                    currentZombies = zombiesConfig.normalZombies; break;
            
                case Difficulty.Hard:
                    currentZombies = zombiesConfig.hardZombies; break;
            
                case Difficulty.Nightmare:
                    currentZombies = zombiesConfig.nightmareZombies; break;
            }
            #endregion
            
            currentEnemyCount = baseEnemyCount;
        }

        private void Start()
        {
            StartNewWave();
        }
        private void OnEnable()
        {
            OnStartNewWave += Wave;
            OnEndWave += EndWave;

            OnEnemyDeath += OnEnemyDied;
            OnEnemyDeath += CheckWave;
        }

        private void OnDisable()
        {
            OnStartNewWave -= Wave;
            OnEndWave -= EndWave;

            OnEnemyDeath -= OnEnemyDied;
            OnEnemyDeath -= CheckWave;
        }

        public void StartNewWave() => OnStartNewWave?.Invoke();
        private static void OnEnemyDied() => currentEnemyCount--;

        private static void CheckWave()
        {
            if (currentEnemyCount <= 0)
            {
                OnEndWave?.Invoke();
            }        
        }

        private void EndWave()
        {
            baseEnemyCount = Mathf.RoundToInt(baseEnemyCount * enemyCountIncreaseRate);
            currentEnemyCount = baseEnemyCount;
            currentWaveNumber++;
            currentExplosiveEnemyAmount = 0;
            Invoke(nameof(StartNewWave), waveDelay);
        }
        private void ChangeSelectedSpawnPoint()
        {
            if (selectedSpawnerIndex >= spawnPoints.Length - 1)
                selectedSpawnerIndex = 0;
            else
                selectedSpawnerIndex++;
        }
        private void Wave()
        {
            print($"{currentWaveNumber} Wave, Zombie: {currentEnemyCount}");
            
            StartCoroutine(Spawning());
            return;

            IEnumerator Spawning()
            {
                for (int i = currentEnemyCount; i > 0; i--)
                {
                    SpawnRandomEnemy(spawnPoints[selectedSpawnerIndex], currentZombies);
                    ChangeSelectedSpawnPoint();
                    yield return new WaitForSeconds(0.2f);
                }
                yield return null;
            }
        }

        private void SpawnRandomEnemy(Transform spawnPoint, Zombie[] enemes)
        {
            int totalProbability = 0;
            foreach (Zombie zombie in enemes)
            {
                totalProbability += zombie.probability;
            }
            
            int randomNumber = Random.Range(0, totalProbability);
            
            foreach (Zombie zombie in enemes)
            {
                if (randomNumber < zombie.probability)
                {
                    if (zombie.zombie.typesOfEnemy is TypesOfEnemy.Explosive)
                    {
                        currentExplosiveEnemyAmount++;
                    }
                    
                    if (currentExplosiveEnemyAmount > maxExplosiveEnemyAmount)
                    {
                        SpawnRandomEnemy(spawnPoint, enemes);
                        return;
                    }
                    
                    SpawnEnemy(zombie.zombie, spawnPoint);
                    return;
                }

                randomNumber -= zombie.probability;
            }
        }
        private static void SpawnEnemy(Enemy enemy, Transform spawnPoint)
        {
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        }
    }
}