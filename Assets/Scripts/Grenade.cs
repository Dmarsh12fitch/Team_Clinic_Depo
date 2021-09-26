using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionEffect;
    public float blastRadius = 3f;
    public float damage = 30f;

    private bool hasBeenTriggered;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(grenadeTicking());
    }

    IEnumerator grenadeTicking()
    {
        yield return new WaitForSeconds(4f);
        if(!hasBeenTriggered){
            explodeGrenade();
        }
    }

    void explodeGrenade()
    {
        hasBeenTriggered = true;
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach(Collider affectedObject in colliders)
        {
            if (affectedObject.gameObject.CompareTag("Player"))
            {
                affectedObject.gameObject.GetComponent<PlayerController>().takeDamage(damage);
            }
        }

        Destroy(gameObject, 0.5f);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!hasBeenTriggered)
            {
                explodeGrenade();
            }
        }
    }
}
