using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float range;

    public Camera playerCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }


    }


    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            hit.transform.GetComponent<Enemy>();

            Enemy e = hit.transform.GetComponent<Enemy>();
            if(e != null)
            {
                e.enemyHit(damage);
            }
        }
    }

}
