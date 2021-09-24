using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(grenadeTicking());
    }

    IEnumerator grenadeTicking()
    {
        yield return new WaitForSeconds(4f);
        explodeGrenade();
    }

    void explodeGrenade()
    {
        Debug.Log("EXPLODE HERE!!!");


        Destroy(gameObject, 0.5f);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            explodeGrenade();
        }
    }
}
