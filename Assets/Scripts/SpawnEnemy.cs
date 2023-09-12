using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefabs;
    [SerializeField] float spawnRadius = 25.0f;
    [SerializeField] float moveSpeed = 2.0f;

    private Vector3 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
        InvokeRepeating("SpawnEnemies", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        Vector3 randomPos = Random.insideUnitSphere * spawnRadius;
        Vector3 enemyPos = new Vector3(spawnPosition.x + randomPos.x, 0f, spawnPosition.z + randomPos.z);

        GameObject enemy = Instantiate(enemyPrefabs, enemyPos, transform.rotation);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.moveSpeed = moveSpeed;
        enemyMovement.targetPos = spawnPosition;
        enemy.transform.LookAt(spawnPosition);
    }
}
