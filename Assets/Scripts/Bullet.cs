using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 70f;

    private Transform target;
    private Collider col;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        //if(dir.magnitude <= distanceThisFrame) // already hit the target
        //{
        //    HitTarget();
        //    return;
        //}

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }


    private void OnTriggerEnter(Collider other)
    {
        HitTarget();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitTarget();
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.OnTriggerEnter(col);
        }
        
    }

    void HitTarget()
    {
        Destroy(gameObject);
    }
}
