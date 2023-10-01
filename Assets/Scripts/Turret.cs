using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float shootingInterval = 1.0f;
    [SerializeField] float shootingRange = 5f;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Transform partRotate;

    private List<GameObject> enemies = new List<GameObject>();
    private float nextShotTime;
    private float scanRadius = 5f;
    GameObject nearestEnemy;

    private void Update()
    {
        detectedEnemies();

        nearestEnemy = GetNearestEnemy();

        if(nearestEnemy == null)
        {
            return;
                
        }

        Vector3 direction = nearestEnemy.transform.position - transform.position;

        //direction.y = 0;

        if (!nearestEnemy.GetComponent<EnemyMovement>().isDead)
        {
            if (Time.time > nextShotTime && direction.magnitude <= shootingRange)
            {
                ShootBullet(nearestEnemy.transform, direction);
                nextShotTime = Time.time + shootingInterval;
            }

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = lookRotation.eulerAngles;
            partRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
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

    void ShootBullet(Transform target, Vector3 direction)
    {
        GameObject bulletGO = Instantiate(arrowPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(direction));
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
