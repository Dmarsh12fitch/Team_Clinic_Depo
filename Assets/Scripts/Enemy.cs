using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float health;
    public float movementSpeed;
    private Rigidbody rb;

    public LayerMask whatIsGround, whatIsPlayer;

    //While Patroling Area
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //For Attacking Player
    public float timeBetweenEveryAtack;
    bool hasAttackedAlready;

    //For Moving Around
    private Vector3 destination;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private bool isFloating;


    //Grenade
    public GameObject grenade;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if(playerInSightRange && playerInSightRange)
        {
            AttackPlayer();
        }

        Vector3 goTo = new Vector3(destination.x, transform.position.y, destination.z);
        transform.LookAt(goTo);
        //Debug.Log(goTo);
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        if(transform.position.y < 1.5f || isFloating)
        {
            floatUp();
        }



    }

    //Float up
    private void floatUp()
    {
        isFloating = true;
        if (transform.position.y < 2.5f)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
        } else
        {
            isFloating = false;
        }
        
    }



    //States
    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        } else
        {
            destination = walkPoint;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

    }

    private void ChasePlayer()
    {
        destination = player.position;
    }

    private void AttackPlayer()
    {
        //enemy temporarily stops to attack
        destination = transform.position;

        transform.LookAt(player);

        if (!hasAttackedAlready)
        {
            Vector3 spawnLoco = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Rigidbody rbg = Instantiate(grenade, spawnLoco, Quaternion.identity).GetComponent<Rigidbody>();
            rbg.AddForce(transform.forward * 15f, ForceMode.Impulse);
            rbg.AddForce(transform.up * 4f, ForceMode.Impulse);
            Debug.Log("Shoot Shoot!");

            hasAttackedAlready = true;
            Invoke(nameof(ResetAttack), timeBetweenEveryAtack);
        }
    }


    //Other
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ResetAttack()
    {
        hasAttackedAlready = false;
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
