using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float visionRadius = 10f;
    public float attackDistant = 2f;
    public float moveSpeed = 2.0f; 
    public Vector3 targetPos;
    
    private float rotationSpeed = 5f;
    private Animator animator;
    private Rigidbody rb;
    public Transform target;
    private CoinSpawn coinSpawner;

    public bool isAttacking;
    public bool isDead;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        coinSpawner = GetComponent<CoinSpawn>();
    }

    private void Update()
    {
        //if (target == null)
        //{
        //    float distanceToTarget = Vector3.Distance(transform.position, targetPos);

        //    if (distanceToTarget > 0.2f)
        //    {
        //        Vector3 moveDirection = (targetPos - transform.position).normalized;

        //        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        //    }
        //}
        if(!isAttacking)
        {
            FindNewTarget();
        }
        if(target != null)
        {
            if (isAttacking)
            {
                Tile component = target.GetComponent<Tile>();
                if ((object)base.gameObject != component.attacker)
                {
                    isAttacking = false;
                }
            }
            else
            {
                if (Vector3.Distance(base.transform.position, target.position) < attackDistant && !isAttacking)
                {
                    Attack();
                }
            }
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
            }
            FindNewTarget();
            //Attack();
        }
        animator.SetBool("hasTarget", target != null);
        animator.SetBool("Attacking", isAttacking);

    }

    private void FixedUpdate()
    {
        Vector3 normalized = (targetPos - base.transform.position).normalized;
        if (target != null)
        {
            normalized = (target.position - base.transform.position).normalized;
            Debug.DrawRay(base.transform.position, target.position - base.transform.position, Color.red);
        }
        normalized *= moveSpeed;
        Vector3 velocity = new Vector3(normalized.x, rb.velocity.y, normalized.z);
        rb.velocity = velocity;
        if (normalized.sqrMagnitude > 0.01f)
        {
            float y = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
            Quaternion b = Quaternion.Euler(0f, y, 0f);
            base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grid"))
        {
            Collider otherColliderToIgnore = collision.gameObject.GetComponent<Collider>();
            Physics.IgnoreCollision(GetComponent<Collider>(), otherColliderToIgnore);
            
        }

    }

    void FindNewTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, visionRadius);
        float closestDistance = Mathf.Infinity;
        
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.CompareTag("Tile") && !hit.GetComponent<Tile>().isBeingAttacked)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    target = hit.transform;
                }
            }
        }
        //Debug.Log(target);
        //if(target != null)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);

        //    if(Vector3.Distance(transform.position, target.position) < closestDistance)
        //    {
        //        Attack();
        //    }
        //}
    }

    void Attack()
    {
        if (target != null)
        {
            Tile component = target.GetComponent<Tile>();
            if (!component.isBeingAttacked)
            {
                isAttacking = true;
                component.isBeingAttacked = true;
                component.attacker = base.gameObject;
            }
            else
            {
                FindNewTarget();
            }
        }
        else
        {
            FindNewTarget();
        }
    }

    public void Die()
    {
        isDead = true;
        coinSpawner.spawnPosition = base.transform.position + Vector3.up * 2f;
        coinSpawner.isSpawnCoin = true;
        if (target != null)
        {
            Tile component = target.GetComponent<Tile>();
            component.attacker = null;
            component.isBeingAttacked = false;
        }
    }


}