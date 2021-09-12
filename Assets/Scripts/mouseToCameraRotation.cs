using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseToCameraRotation : MonoBehaviour
{
    public float mouseSenseitivity = 500f;

    public Transform player;

    float xRotate = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseMovementX = Input.GetAxis("Mouse X") * mouseSenseitivity * Time.deltaTime;
        float mouseMovementY = Input.GetAxis("Mouse Y") * mouseSenseitivity * Time.deltaTime;
        xRotate -= mouseMovementY;
        xRotate = Mathf.Clamp(xRotate, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);
        player.Rotate(Vector3.up * mouseMovementX);
        
    }
}
