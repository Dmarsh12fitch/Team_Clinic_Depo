using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    

    // Start is called before the first frame update
    void Start()
    {
        
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
        //disable this enemy's functionality
        //do death anim
        Destroy(gameObject, 1);
    }



}
