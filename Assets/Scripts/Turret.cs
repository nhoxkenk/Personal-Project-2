using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootingInterval = 2.0f;
    [SerializeField] float shootingRange = 10f;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Transform partRotate;
    [SerializeField]
    private float bulletSpeed = 20f;


    private Animator animator;
    private List<GameObject> enemies = new List<GameObject>();
    private float nextShotTime;
    private float scanRadius = 5f;
    GameObject nearestEnemy;
    private bool isPlace = true;
    private bool hasDelayed = false;
    private float delayTimer = 1.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        FindObjectOfType<AudioManager>().Play("PlaceTurret");
    }

    private void Update()
    {
        Place();

        detectedEnemies();

        nearestEnemy = GetNearestEnemy();

        if (nearestEnemy == null)
        {
            return;

        }
        else
        {
            Vector3 direction = nearestEnemy.transform.position - transform.position;

            if (!nearestEnemy.GetComponent<EnemyMovement>().isDead)
            {
                if (Time.time > nextShotTime && direction.magnitude <= shootingRange && !isPlace)
                {
                    FindObjectOfType<AudioManager>().Play("TurretShoot");
                    ShootBullet(nearestEnemy.transform, direction);
                    nextShotTime = Time.time + shootingInterval;
                }

                Quaternion lookRotation = Quaternion.LookRotation(direction);
                Vector3 rotation = lookRotation.eulerAngles;
                partRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
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

    void Place()
    {
        if (!hasDelayed)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0)
            {
                isPlace = false;
                hasDelayed = true;
            }
        }
    }

    GameObject GetNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestEnemyDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < nearestEnemyDistance)
            {
                nearestEnemy = enemy;
                nearestEnemyDistance = distance;
            }
        }

        return nearestEnemy;
    }

    void ShootBullet(Transform target, Vector3 direction)
    {
        Object.Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Bullet>().Shoot(direction.normalized, bulletSpeed);
        //arrow.transform.Translate(direction.normalized * Time.deltaTime * 70, Space.World);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scanRadius);
    }

    
}
