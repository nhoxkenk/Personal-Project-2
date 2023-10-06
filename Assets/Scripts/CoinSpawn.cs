using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    public GameObject coinPrefabs;
    public float spawnRate = 0.2f;
    public int numCoin = 10;
    public Vector3 spawnPosition;
    public bool isSpawnCoin;
    public GameManager manager;
    private float timeSinceLastSpawn;
    private int coinSpawned;

    // Start is called before the first frame update
    void Start()
    {
        coinSpawned = 0;
        timeSinceLastSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawnCoin)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn > spawnRate && coinSpawned < numCoin)
            {
                manager.GetCoin(1);
                Instantiate(coinPrefabs, spawnPosition, Quaternion.identity);
                coinSpawned++;
                timeSinceLastSpawn = 0;
            }
        }
    }
}
