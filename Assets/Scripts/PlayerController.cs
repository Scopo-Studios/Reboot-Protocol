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
    private float mouseSensitivity = 150f;
    private Animator animator;
    private Vector3 oldPosition;
    public AudioClip hurtSound;
    public AudioClip gunSound;
    public AudioClip missSound;
    public AudioClip footStepSound;
    public float footStepDelay;
    private float nextFootstep = 0;
    private bool delay;
    public bool use;
    public bool pickUp;
    private bool shootCD = true;
    private AnimatorStateInfo stateInfo;
    public int health = 100; // Max 100, min 0

    public bool canShoot = true; // <--- NEW FLAG FOR SHOOT CONTROL

    public float range = 100f;
    public Camera cam;

    public LayerMask enemyLayer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 = Base Layer
        delay = true;
        use = false;
        pickUp = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pickUp)
        {
            StartCoroutine(PickUp());
        }
        else if (Input.GetKeyDown(KeyCode.F) && use)
        {
            StartCoroutine(Use());
        }
        else if (canShoot && Input.GetMouseButtonDown(0) && shootCD == true) // <--- UPDATED
        {
            StartCoroutine(Shoot());
            StartCoroutine(ShootCoolDown());
        }
        else if (Input.GetKeyDown(KeyCode.R)){
            StartCoroutine(Reload());
        }

        if (delay)
        {
            MoveAndAnimation();
        }

        Look();
    }

    void MoveAndAnimation()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        animator.SetFloat("Y", verticalInput);
        animator.SetFloat("X", horizontalInput);

        if (verticalInput != 0 || horizontalInput != 0)
        {
            animator.SetBool("RunStop", true);
        }
        else
        {
            animator.SetBool("RunStop", false);
        }

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

    void Look()
    {
        mouseInputV = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseInputH = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseInputV);
    }

    private IEnumerator Use()
    {
        delay = false;
        animator.SetTrigger("Use");
        yield return new WaitForSeconds(1.4f);
        delay = true;
    }

    private IEnumerator PickUp()
    {
        delay = false;
        animator.SetTrigger("Pickup");
        yield return new WaitForSeconds(1.2f);
        delay = true;
    }

    private IEnumerator Reload()
    {
        delay = false;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(0.8f);
        delay = true;
    }

    private IEnumerator Shoot()
    {
        GetComponent<AudioSource>().PlayOneShot(gunSound, 0.7f);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        delay = false;
        animator.SetTrigger("Shoot");

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
        else
        {
            StartCoroutine(MissSound());
        }

        yield return new WaitForSeconds(0.3f);
        delay = true;
    }

    private IEnumerator MissSound()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<AudioSource>().PlayOneShot(missSound, 0.7f);
    }

    private IEnumerator ShootCoolDown()
    {
        shootCD = false;
        yield return new WaitForSeconds(1f);
        shootCD = true;
    }

    public void TakeDamage(int damage)
    {
        if (delay && health > 0)
        {
            health -= damage;
            StartCoroutine(GotHit());
        }
    }

    private IEnumerator GotHit()
    {
        GetComponent<AudioSource>().PlayOneShot(hurtSound, 1.5f);
        animator.SetTrigger("Hit");
        delay = false;

        if (health <= 0)
        {
            Debug.Log("Player died!");
        }

        yield return new WaitForSeconds(0.5f);
        delay = true;
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > 100)
        {
            health = 100;
        }
        Debug.Log("Healed! Current Health: " + health);
    }
}