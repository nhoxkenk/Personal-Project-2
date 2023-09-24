using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject tileObject;

    public Vector2 coordinates;

    private TileHealth health;

    public bool isBeingAttacked;

    public GameObject attacker;

    //public TileHealth health;

    private bool needsToUpdateCracks;

    private float prevHealthAmount;

    private void Start()
    {
        health = GetComponent<TileHealth>();
        prevHealthAmount = health.amount;
    }

    private void Update()
    {
        if (isBeingAttacked && attacker == null)
        {
            isBeingAttacked = false;
        }
        if (health.amount != prevHealthAmount)
        {
            prevHealthAmount = health.amount;
        }
    }

    public void Sink()
    {
        if (attacker != null)
        {
            EnemyMovement component = attacker.GetComponent<EnemyMovement>();
            component.target = null;
            component.isAttacking = false;
            attacker = null;
            isBeingAttacked = false;
            Destroy(gameObject);
            Destroy(tileObject);
        }
    }

}
