using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public float attackDuration = 0.5f;

    public int damageAmount = 1;

    private Animator animator;

    [SerializeField]
    private CapsuleCollider weaponCollider;

    [SerializeField]
    private float cooldown = 0.5f;

    private float attackTimer;

    private bool canAttack = true;

    [SerializeField]
    private Damage damage;

    //public Transform tip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        weaponCollider.enabled = false;
        damage.amount = damageAmount;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            canAttack = false;
            attackTimer = 0f;
            animator.Play("Spear_shot");
        }
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= cooldown)
            {
                canAttack = true;
                weaponCollider.enabled = false;
                animator.Play("Put Away");
            }
        }
    }
}
