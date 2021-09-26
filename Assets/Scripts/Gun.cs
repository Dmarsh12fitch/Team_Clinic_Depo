using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float range;
    public float ammo;
    public float fireRate;
    public float ableToFireTime = 0f;
    public GameObject display;
    public bool isPlayersGun;
    public ParticleSystem muzzleFlash;

    public Camera playerCamera;


    public void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range) && ammo > 0)
        {
            //delete this when done playtesting
            //Debug.Log(hit.transform.name);

            //do firing animation here
            muzzleFlash.Play();

            if (isPlayersGun)
            {
                Enemy target = hit.transform.GetComponent<Enemy>();
                if(target != null)
                {
                    target.takeDamage(damage);
                }

            } else
            {
                //enemy firing at the player
            }
        }
    }

}
