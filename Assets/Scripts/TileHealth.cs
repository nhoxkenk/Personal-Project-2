using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHealth : MonoBehaviour
{
    public float maxHealth = 100f;

    public float amount;

    public string[] damageTags;

    public GameObject invisibleWallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        amount = maxHealth;
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
        Tile component = GetComponent<Tile>();
        Vector3 posInvisible = new Vector3 (base.transform.position.x, base.transform.position.y + 1, base.transform.position.z);
        Object.Instantiate(invisibleWallPrefab, posInvisible, Quaternion.identity, component.transform.parent);
        component.Sink();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        string[] array = damageTags;
        foreach (string text in array)
        {
            if (other.CompareTag(text))
            {
                Debug.Log(text);
                Damage component2 = other.transform.GetComponent<Damage>();
                TakeDamage(component2.amount);
            }
        }
    }

}
