using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float speed = 5.0f;
    private float horizontalInput;
    private float verticalInput;
    private float mouseInputV;
    private float mouseInputH;
    private float mouseSensitivity = 100f;
    private Animator animator;
    private Vector3 oldPosition;
    public AudioClip hurtSound;
    public AudioClip gunSound;
    public AudioClip footStepSound;
    public float footStepDelay;
    private float nextFootstep = 0;
    private bool delay;
    public bool use;
    public bool pickUp;
    private bool shootCD = true;
    private AnimatorStateInfo stateInfo;
    public int health = 5;


    public float range = 100f;
    public Camera cam;

    public LayerMask enemyLayer;
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
        else if (Input.GetMouseButtonDown(0) && shootCD == true){
            StartCoroutine(Shoot());
            StartCoroutine(ShootCoolDown());
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

    private IEnumerator Shoot(){
        GetComponent<AudioSource>().PlayOneShot(gunSound, 0.7f);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        delay = false;
        animator.SetFloat("Y", 0);
        animator.SetFloat("X", 0);
        animator.SetBool("RunStop", true);
        if (Physics.SphereCast(ray, 0.7f, out hit, range, enemyLayer))
        {
            
            PatrolEnemy enemy = hit.transform.GetComponent<PatrolEnemy>();
            if (enemy != null)
            {
                
                enemy.TakeDamage();
            }
            SleepEnemy enemy2 = hit.transform.GetComponent<SleepEnemy>();
            if (enemy2 != null)
            {
                
                enemy2.TakeDamage();
                
            }
        }
        yield return new WaitForSeconds(0.3f);
        delay = true;
    }

    private IEnumerator ShootCoolDown(){
        shootCD = false;
        yield return new WaitForSeconds(1f);
        shootCD = true;
    }

    

    private IEnumerator GotHit(){
        GetComponent<AudioSource>().PlayOneShot(hurtSound, 1.5f);
        delay = false;
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        delay = true;
    }

    public void TakeDamage(int damage)
    {

        if (delay) StartCoroutine(GotHit());
        health -= damage;
        Debug.Log("Player took " + damage + " damage! Health: " + health);
        


        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        /* Player.GetComponent<ThirdPersonConroller)()enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        hud.SetActive(false);
        inv.SetActive(false);
        deathScreen.SetActive(true); */

    }


    public void Heal(int amount)
    {
        health += amount;

        // Cap health at 5
        if (health > 5)
        {
            health = 5;
        }

        Debug.Log("Healed! Current Health: " + health);
    }

}
