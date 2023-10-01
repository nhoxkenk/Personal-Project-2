using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;

    public float amount;

    public string[] damageTags;

    public GameObject invisibleWallPrefab;

    private EnemyMovement enemy;
    // Start is called before the first frame update
    void Start()
    {
        amount = maxHealth;
        enemy = GetComponent<EnemyMovement>();
    }
    public void TakeDamage(float amount)
    {
        this.amount -= amount;
        if (this.amount <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        EnemyMovement component = GetComponent<EnemyMovement>();
        if (component != null)
        {
            component.Die();
            GetComponent<Animator>().SetTrigger("Die");
            Object.Destroy(base.gameObject, 3f);
        }
        else
        {
            Object.Destroy(base.gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        string[] array = damageTags;
        foreach (string text in array)
        {
            if (other.CompareTag(text))
            {
                Damage component = other.GetComponent<Damage>();
                TakeDamage(component.amount);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
