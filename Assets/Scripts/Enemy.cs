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
    private bool isDead;

    public LayerMask whatIsGround, whatIsPlayer;

    //While Patroling Area
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    private float tooMuchTimeWalking = 4f;

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

        destination = new Vector3(destination.x, transform.position.y, destination.z);
        transform.LookAt(destination);
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        if (transform.position.y < 1.5f || isFloating)
        {
            floatUp();
        }

        tooMuchTimeWalking -= Time.deltaTime;

        if(distanceToWalkPoint.magnitude < 1f || tooMuchTimeWalking <= 0)
        {
            walkPointSet = false;
            tooMuchTimeWalking = 4f;
        }

    }

    private void ChasePlayer()
    {
        destination = player.position;
        destination = new Vector3(destination.x, transform.position.y, destination.z);
        transform.LookAt(destination);
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        if (transform.position.y < 1.5f || isFloating)
        {
            floatUp();
        }
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
            float randomX = Random.Range(10f, 20f);
            float randomY = Random.Range(3f, 6f);
            rbg.AddForce(transform.forward * randomX, ForceMode.Impulse);
            rbg.AddForce(transform.up * randomY, ForceMode.Impulse);

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
        if(health <= 0 && !isDead)
        {
            isDead = true;
            deathSequence();
        }
    }


    void deathSequence()
    {
        GameObject.Find("Player").gameObject.GetComponent<PlayerController>().enemiesKilled += 1f;
        rb.useGravity = true;
        //disable this enemy's functionality
        //do death anim
        Destroy(gameObject, 0.6f);
    }



}
