using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

using Random = UnityEngine.Random;

public class EnemesSpawner : MonoBehaviour
{
    private void OnValidate()
    {
        if (firstBorder >= secondBorder)
            secondBorder++;

        if (secondBorder <= firstBorder)
            firstBorder--;

        WD = waveDelay;
    }

    [Header("Wave Settings")]
    [SerializeField][Range(0, 19)] private int firstBorder;
    [SerializeField][Range(0, 20)] private int secondBorder;
    [Range(1f, 100f)] public float waveDelay;

    [Header("Types of zombies")]
    [SerializeField] private GameObject defaultZombie;
    [SerializeField] private GameObject zombieSprinter;
    [SerializeField] private GameObject zombieTank;
    [SerializeField] private GameObject toxicZombie;
    [SerializeField] private GameObject explosiveZombie;
    [SerializeField] private GameObject zombieShooter;
    [SerializeField] private GameObject bonusZombie;

    [Header("Difficulty")]
    public Difficulty difficulty;

    private static float WD;
    public static event Action OnStartNewWave;
    public static event Action OnEndNewWave;
    public static Action OnEnemyDeath;
    private event Action Wave;
    private int complexityScale = 1;
    public enum Difficulty { easy, normal, hard, nightmare }
    private int maxCount;
    private float timer;
    private static int neededEnemyDeathCounter;
    public static int needEnemy
    {
        get { return neededEnemyDeathCounter; }
        set
        {
            if (value < 0)
                return;

            neededEnemyDeathCounter = value;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Vector3.one / 2);
    }

    private void OnEnable()
    {
        OnStartNewWave += ChooseWave;
        OnEndNewWave += EndWave;
        OnEnemyDeath += OnEnemyDied;
        OnEnemyDeath += CheckWave;
        maxCount = secondBorder;

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
        OnEndNewWave -= EndWave;
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
        if (needEnemy <= 0)
        {
            if (timer > 5f)
            {
                OnEndNewWave?.Invoke();
                timer = 0;
            }
        }
    }
    public void AddNeededEnemyDeath()
    {
        needEnemy++;
    }

    private void OnEnemyDied()
    {
        needEnemy--;
    }
    private void EndWave()
    {
        complexityScale++;
        WaitWave(WD);
    }

    private void ChooseWave()
    {
        Wave?.Invoke();      
    }

    #region WaitWave
    private void WaitWave(float _wd)
    {
        Invoke(nameof(Inv), _wd);
    }
    private void Inv()
    {
        OnStartNewWave?.Invoke();
    }
    #endregion

    #region Difficulty Presets
    private void EasyMode()
    {
        for (int i = 0; i < complexityScale; i++)
        {
            if (complexityScale < firstBorder)
            {
                RandomSpawnEnemy(defaultZombie, zombieSprinter);
                BonusCategory();
            }
            else
            {
                if (firstBorder - 1 < complexityScale && complexityScale < secondBorder)
                {
                    RandomSpawnEnemy(defaultZombie, zombieSprinter, toxicZombie, zombieTank);
                    BonusCategory();
                }
            }
        }

        if (complexityScale > secondBorder - 1)
        {
            for (int i = 0; i < maxCount; i++)
            {
                RandomSpawnEnemy(defaultZombie, zombieSprinter, toxicZombie, zombieTank, explosiveZombie, zombieShooter);
            }
            BonusCategory();
            maxCount++;
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
    private void BonusCategory()
    {
        int type = Random.Range(0, 5);

        if (type == 0)
            AddEnemy(bonusZombie);
        else
            return;
    }


    private void RandomSpawnEnemy(params GameObject[] enemes)
    {
        int type = Random.Range(0, enemes.Length);

        AddEnemy(enemes[type]);
    }
    private void AddEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
        AddNeededEnemyDeath();
    }

    #endregion
    private void Start()
    {
        WaitWave(WD);
    }
    private bool isPause = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPause = !isPause;
            EditorApplication.isPaused = isPause;
        }

        timer += Time.deltaTime;

        Debug.Log(needEnemy);
    }
}
