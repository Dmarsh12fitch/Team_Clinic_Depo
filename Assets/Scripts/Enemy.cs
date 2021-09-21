using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        //do some sort of effect
        health -= damage;
        if(health <= 0)
        {
            deathSequence();
        }
    }


    void deathSequence()
    {
        rb.useGravity = true;
        //disable this enemy's functionality
        //do death anim
        Destroy(gameObject, 0.6f);
    }



}
