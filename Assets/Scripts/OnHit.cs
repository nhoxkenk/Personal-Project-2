using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(1);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(2);
            Destroy(collision.gameObject);
        }
    }
}
