using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float shootingInterval = 1.0f;
    [SerializeField] float shootingRange = 10f;
    [SerializeField] Transform arrowSpawnPoint;

    private List<GameObject> enemies = new List<GameObject>();
    private float nextShotTime;
    private float scanRadius = 10f;


    private void Update()
    {
        detectedEnemies();
        Debug.Log(enemies.Count);
        if(enemies.Count > 0)
        {
            GameObject nearestEnemy = GetNearestEnemy();

            if(nearestEnemy != null)
            {
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                direction.y = 0;
                if(Time.time > nextShotTime && direction.magnitude <= shootingRange)
                {
                    ShootArrow(nearestEnemy.transform, direction);
                    nextShotTime = Time.time + shootingInterval;
                }
            }
        }

    }

    void detectedEnemies()
    {
        enemies.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, scanRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                enemies.Add(col.gameObject);
            }
        }
    }

    GameObject GetNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestEnemyDistance = float.MaxValue;

        foreach(GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);

            if(distance < nearestEnemyDistance)
            {
                nearestEnemy = enemy;
                nearestEnemyDistance = distance;
            }
        }

        return nearestEnemy;
    }

    void ShootArrow(Transform target, Vector3 direction)
    {
        GameObject bulletGO = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.LookRotation(direction));
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if(bullet != null)
        {
            bullet.Seek(target);
        }
        //arrow.transform.Translate(direction.normalized * Time.deltaTime * 70, Space.World);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scanRadius);
    }

}
