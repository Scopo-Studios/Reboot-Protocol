using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float speed = 5.0f;
    private float horizontalInput;
    private float verticalInput;
    private float leftClick;
    private float mouseInputV;
    private float mouseInputH;
    private float mouseSensitivity = 100f;
    private Animator animator;
    private Vector3 oldPosition;
    public AudioClip footStepSound;
    public float footStepDelay;
    private float nextFootstep = 0;
    private bool delay;
    public bool use;
    public bool pickUp;
    private AnimatorStateInfo stateInfo;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 = Base Layer
        delay = true;
        use = false;
        pickUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && pickUp){
            StartCoroutine(PickUp());
        }
        else if (Input.GetKeyDown(KeyCode.F) && use){
            StartCoroutine(Use());
        }
        if (delay){
            MoveAndAnimation();
        }
        Look();
    }

    void MoveAndAnimation(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        
        
        animator.SetFloat("Y", verticalInput);
        animator.SetFloat("X", horizontalInput);
        if (verticalInput != 0 || horizontalInput != 0){
            animator.SetBool("RunStop", true);
        }
        else {
            animator.SetBool("RunStop", false);
        }
        
       
        
        
        //Should change these to Rigidbody forces for movement for better collisions maybe
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
        

        
        
        if (verticalInput != 0 || horizontalInput != 0)
        {
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0) 
            {
                GetComponent<AudioSource>().PlayOneShot(footStepSound, 0.7f);
                nextFootstep += footStepDelay;
            }
        }
    }

    void Look(){
        mouseInputV = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseInputH = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseInputV);
    }
    private IEnumerator Use(){
        delay = false;
        animator.SetTrigger("Use");
        yield return new WaitForSeconds(1.4f);
        delay = true;
    }

    private IEnumerator PickUp(){
        delay = false;
        animator.SetTrigger("Pickup");
        yield return new WaitForSeconds(1.2f);
        delay = true;
    }

    
}
