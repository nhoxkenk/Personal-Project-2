using System;
using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        SPAWNING = 0,
        WAITING = 1,
        COUNTING = 2
    }

    [Serializable]
    public class Wave
    {
        public string name;

        public Transform enemy;

        public int count;

        public float rate;
    }

    public GameManager gameManager;

    public int maxEnemies = 20;

    public AnimationCurve wavesDifficulty;

    public CoinSpawn coinSpawner;

    public Wave[] waves;

    public Wave[] initialWaves;

    private int nextWave;

    private int currWave = 1;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;

    private float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    public int NextWave => nextWave + 1;

    public float WaveCountdown => waveCountdown;

    public SpawnState State => state;

    private void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced.");
        }
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (EnemyIsAlive())
            {
                return;
            }
            WaveCompleted();
        }
        if (waveCountdown <= 0f)
        {
            if (state != 0)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    public void ResetWaves(float difficulty)
    {
        int num = 0;
        waves = new Wave[initialWaves.Length];
        Wave[] array = initialWaves;
        foreach (Wave wave in array)
        {
            float num2 = num;
            Wave wave2 = new Wave();
            wave2.count = (int)(difficulty * (float)maxEnemies * wavesDifficulty.Evaluate(num2 / (float)(waves.Length - 1)));
            wave2.enemy = wave.enemy;
            wave2.rate = wave.rate;
            waves[num] = wave2;
            num++;
        }
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        currWave++;
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        if (nextWave + 1 > waves.Length - 1)
        {
            Debug.Log("ALL WAVES COMPLETE! Looping...");
            nextWave = 0;
            currWave = 1;
           // gameManager.LevelCompleted();
        }
        else
        {
            nextWave++;
        }
    }

    private bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;
        //gameManager.ShowWaveText(currWave, waves.Length);
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        state = SpawnState.WAITING;
    }

    private void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform transform = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        CoinSpawn component = UnityEngine.Object.Instantiate(_enemy, transform.position, transform.rotation).GetComponent<CoinSpawn>();
        component.manager = gameManager;
        component.target = Vector3.zero;
    }
}
