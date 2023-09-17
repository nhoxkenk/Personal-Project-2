using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefabs;
    [SerializeField] float spawnRadius = 5.0f;
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField]
    private float spawnRate = 0.5f;

    private Vector3 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
        InvokeRepeating("SpawnEnemies", 0.0f, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        float y = Random.Range(0f, 360f);

        Vector3 enemyPos = base.transform.position + Quaternion.Euler(0f, y, 0f) * Vector3.forward * spawnRadius;

        GameObject enemy = Instantiate(enemyPrefabs, enemyPos, transform.rotation);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.moveSpeed = moveSpeed;
        enemyMovement.targetPos = spawnPosition;
        enemy.transform.LookAt(spawnPosition);
    }
}
