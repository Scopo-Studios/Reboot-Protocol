using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float speed = 20.0f;
    private float horizontalInput;
    private float verticalInput;
    private float mouseInputV;
    private float mouseInputH;
    private float mouseSensitivity = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mouseInputV = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseInputH = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        
        //Should change these to Rigidbody forces for movement for better collisions
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
        

        //Glitchy still
        transform.Rotate(Vector3.up * mouseInputV);
    }
}
