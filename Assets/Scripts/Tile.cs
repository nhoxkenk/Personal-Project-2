using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject tileObject;

    private TileHealth health;

    public bool isBeingAttacked;

    public GameObject attacker;

    //public TileHealth health;

    private bool needsToUpdateCracks;

    private float prevHealthAmount;

    public MeshRenderer grassRenderer;

    [SerializeField] private Material verySmallCracked;
    [SerializeField] private Material smallCracked;
    [SerializeField] private Material mediumCracked;
    [SerializeField] private Material largeCracked;

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
            UpdateGroundCracks(health.amount);
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

    private void UpdateGroundCracks(float healthAmount)
    {
        Material material = verySmallCracked;
        material = ((healthAmount == health.maxHealth) ? verySmallCracked : ((healthAmount >= 75f) ? smallCracked : ((healthAmount >= 50f) ? mediumCracked : ((!(healthAmount >= 25f)) ? largeCracked : largeCracked))));
        grassRenderer.sharedMaterial = material;
    }

    public float Health()
    {
        return prevHealthAmount;
    }

}
