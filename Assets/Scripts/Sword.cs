using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float swingDuration = 1f;
    public float swingRadius = 90f;
    public int damageAmount = 1;

    private Animator animator;
    private float swingTimer;
    private bool swinging;

    public GameObject gameObjects;
    public GameObject swordBlade;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            swinging = true;
            swingTimer = 0f;
            animator.Play("SwingSword");
            gameObjects.SetActive(true);
        }

        if(swinging)
        {
            swingTimer += Time.deltaTime;
            if(swingTimer >= swingDuration)
            {
                swinging = false;
                gameObjects.SetActive(false);
                animator.Play("Idle");
            }

        }
    }

    

}
