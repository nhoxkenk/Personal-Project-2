using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    //public float speed = 70f;

    //private Transform target;
    //private Collider col;

    //public void Seek(Transform _target)
    //{
    //    target = _target;
    //}

    //private void Start()
    //{
    //    col = GetComponent<Collider>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if(target == null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    Vector3 dir = target.position - transform.position;
    //    float distanceThisFrame = speed * Time.deltaTime;

    //    //if(dir.magnitude <= distanceThisFrame) // already hit the target
    //    //{
    //    //    HitTarget();
    //    //    return;
    //    //}

    //    transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    //}


    //private void OnTriggerEnter(Collider other)
    //{
    //    HitTarget();
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        HitTarget();
    //        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
    //        enemyHealth.OnTriggerEnter(col);
    //    }

    //}

    //void HitTarget()
    //{
    //    Destroy(gameObject);
    //}

    private float bulletSpeed;

    private Vector3 direction;

    private Rigidbody rb;

    private bool isShot;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 2);
    }

    private void Update()
    {
        if (isShot)
        {
            rb.velocity = direction.normalized * bulletSpeed;
        }
    }

    public void Shoot(Vector3 direction, float power)
    {
        this.direction = direction;
        bulletSpeed = power;
        isShot = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            Object.Destroy(base.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Object.Destroy(base.gameObject);
        }
    }
}
