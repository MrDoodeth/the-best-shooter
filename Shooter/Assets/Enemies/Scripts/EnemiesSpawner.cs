using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesSpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField][Min(1)] private int baseEnemyCount = 3;
    [SerializeField] private float enemyCountIncreaseRate = 1.2f;
    [SerializeField] private float waveDelay;
    public Difficulty difficulty;
    public enum Difficulty { easy, normal, hard, nightmare }

    [Header("Types of zombies")]
    [Tooltip("—юда вставл€ть всех зомби в пор€дке возрастани€ их силы (обычный, бегун; танк, токсик)")]
    [SerializeField] private GameObject[] zombies;
    [SerializeField][Range(1, 10)] private int maxExplosiveEnemyAmount;
    private int currentExplosiveEnemyAmount;
    [Space(15)]

    [Tooltip("—юда вставл€ть все точки спавна на карте")]
    [SerializeField] private Transform[] spawnPoints;

    public static Action OnStartNewWave;
    public static Action OnEndWave;
    public static Action OnEnemyDeath;
    private Action Wave;

    private static int currentEnemyCount;
    private int currentWaveNumber = 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;      
        for (int i = 0; i < spawnPoints.Length && spawnPoints != null; i++)
            Gizmos.DrawWireCube(spawnPoints[i].position, Vector3.one / 2);
    }
    private void Start()
    {
        currentEnemyCount = baseEnemyCount;
        OnStartNewWave?.Invoke();
    }

    private void OnEnable()
    {
        OnStartNewWave += ChooseWave;
        OnEndWave += EndWave;

        OnEnemyDeath += OnEnemyDied;
        OnEnemyDeath += CheckWave;

        #region InitDifficulty
        switch (difficulty)
        {
            case Difficulty.easy:
                Wave += EasyMode; break;

            case Difficulty.normal:
                Wave += NormalMode; break;

            case Difficulty.hard:
                Wave += HardMode; break;

            case Difficulty.nightmare:
                Wave += NightmareMode; break;
        }
        #endregion
    }

    private void OnDisable()
    {
        OnStartNewWave -= ChooseWave;
        OnEndWave -= EndWave;

        OnEnemyDeath -= OnEnemyDied;
        OnEnemyDeath -= CheckWave;

        #region ReinitDifficulty
        switch (difficulty)
        {
            case Difficulty.easy:
                Wave -= EasyMode; break;

            case Difficulty.normal:
                Wave -= NormalMode; break;

            case Difficulty.hard:
                Wave -= HardMode; break;

            case Difficulty.nightmare:
                Wave -= NightmareMode; break;
        }
        #endregion
    }
    private void CheckWave()
    {
        if (currentEnemyCount <= 0)
        {
            OnEndWave?.Invoke();
        }        
    }

    public static void OnEnemyDied()
    {
        currentEnemyCount--;
    }   
    private void EndWave()
    {
        baseEnemyCount = Mathf.RoundToInt(baseEnemyCount * enemyCountIncreaseRate);
        currentEnemyCount = baseEnemyCount;
        currentWaveNumber++;
        currentExplosiveEnemyAmount = 0;
        WaitWave(waveDelay);
    }

    private void ChooseWave()
    {
        Wave?.Invoke();
        print($"{currentWaveNumber} Wave, Zombie: {currentEnemyCount}");
    }
    private void WaitWave(float delayInSeconds)
    {
        StartCoroutine(Inv()); 

        IEnumerator Inv()
        {
            yield return new WaitForSeconds(delayInSeconds);
            OnStartNewWave?.Invoke();
            yield return null;
        }
    }

    #region Difficulty Presets

    private int selectedSpawner = 0;

    private void ChangeSelectedSpawnPoint()
    {
        if (selectedSpawner >= spawnPoints.Length - 1)
            selectedSpawner = 0;
        else
            selectedSpawner++;
    }
    private void EasyMode()
    {
        StartCoroutine(Spawning());

        IEnumerator Spawning()
        {
            for (int i = currentEnemyCount; i > 0; i--)
            {
                SpawnRandomEnemy(spawnPoints[selectedSpawner], zombies);
                ChangeSelectedSpawnPoint();
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }

    private void NormalMode()
    {

    }
    private void HardMode()
    {

    }

    private void NightmareMode()
    {

    }
    #endregion

    #region Enemy Spawn
    private void SpawnBonusZombie(Transform spawnPoint)
    {
        int type = Random.Range(0, 5);

        if (type == 0)
            SpawnEnemy(zombies[0], spawnPoint);
        else
            return;
    }


    private void SpawnRandomEnemy(Transform spawnPoint, params GameObject[] enemes)
    {
        int type = Random.Range(0, enemes.Length);
        
        if (type == (enemes.Length - 1))
        {
            currentExplosiveEnemyAmount++;
            if (currentExplosiveEnemyAmount > maxExplosiveEnemyAmount)
            {
                SpawnRandomEnemy(spawnPoint, enemes);
                return;
            }
        }

        SpawnEnemy(enemes[type], spawnPoint);
    }
    private void SpawnEnemy(GameObject enemy, Transform spawnPoint)
    {
        Instantiate(enemy, spawnPoint.position, Quaternion.identity);
    }
    #endregion
}
