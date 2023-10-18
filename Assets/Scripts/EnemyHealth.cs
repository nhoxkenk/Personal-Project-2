using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;

    public float amount;

    public Image healthBar;

    public string[] damageTags;

    public GameObject invisibleWallPrefab;

    public GameObject healthBarComponent;

    private EnemyMovement enemy;

    // Start is called before the first frame update
    void Start()
    {
        amount = maxHealth;
        healthBar.fillAmount = amount/maxHealth;
        enemy = GetComponent<EnemyMovement>();
    }
    public void TakeDamage(float amount)
    {
        this.amount -= amount;
        healthBar.fillAmount = this.amount/ maxHealth;
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
            healthBarComponent.SetActive(false);
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
        //Debug.Log(other.tag);
        string[] array = damageTags;
        foreach (string text in array)
        {
            if (other.CompareTag(text))
            {
                Damage component = other.GetComponent<Damage>();
                FindObjectOfType<AudioManager>().Play("Stab");
                TakeDamage(component.amount);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
