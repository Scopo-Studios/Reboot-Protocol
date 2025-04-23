using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepEnemy : MonoBehaviour
{

    public Transform target;
    private float speed = 3f;
    public float chaseRadius = 15f;
    public float attackRadius = 5f;
    public Rigidbody myRigidbody;
    private bool alerted;
    private bool hit;
    private bool chasing;
    private bool attacking;
    private Animator animator;
    private float rotationSpeed = 10f;
    public float health;
    private bool dead = false;
    
    public bool attackCoolDown = false;
    

    public AudioClip hurtSound;
    public AudioClip footStepSound;
    public float footStepDelay;
    private float nextFootstep = 0;

    // Start is called before the first frame update
    void Start()
    {
        var trigger = GetComponentInChildren<Hit>();
        if (trigger != null)
        {
            trigger.parent = this;
        }
        myRigidbody = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").transform;
        alerted = false;
        chasing = false;
        attacking = false;
        attackRadius = 5f;
        animator = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius){
            alerted = true;
            
        }
        if (Vector3.Distance(target.position, transform.position) <= attackRadius){
            if (!attacking && !attackCoolDown){
                StartCoroutine(Attack());
                StartCoroutine(AttackCoolDown());
            }
        }
        if (alerted){
            speed = 4f;
            if (!chasing){
                StartCoroutine(Rise());
            }
            
            else {
                
                Vector3 direction = target.position - transform.position;
                direction.y = 0f; 
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
                bool isAttackingAnim = animState.IsName("Demon_Throw") || animState.IsName("Demon_Throw-catch");
                if (!hit && !dead && !attacking && !isAttackingAnim){
                    Chase();
                }
            }
                
            
            
        }
        else {
            
        }
    }

    void Chase(){
        
        footStepDelay = 0.4f;
        Vector3 temp = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        myRigidbody.MovePosition(temp);
        nextFootstep -= Time.deltaTime;
        if (nextFootstep <= 0) 
        {
            GetComponent<AudioSource>().PlayOneShot(footStepSound, 2f);
            nextFootstep += footStepDelay;
        }
        
    }

    

    public void TakeDamage(){
        health--;
        GetComponent<AudioSource>().PlayOneShot(hurtSound, 1f);
        
        if (health <= 0 && !dead) {
            dead = true;
            StartCoroutine(Die());
        }
        else if (!hit) StartCoroutine(Hit());  
    }
    private IEnumerator Rise(){
        
        animator.SetBool("Alerted", true);
        yield return new WaitForSeconds(1.3f);
        chasing = true;
    }
    private IEnumerator Hit(){
        hit = true;
        
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.7f);
        
        hit = false;
    }

    private IEnumerator Die(){
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(4f);
        this.gameObject.SetActive(false);
    }

    private IEnumerator Attack()
    {
        
        animator.SetBool("Attacking", true);
        attacking = true;
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(2.1f);
        animator.SetBool("Attacking", false);
        attacking = false;

    }

    private IEnumerator AttackCoolDown()
    {
        attackCoolDown = true;
        yield return new WaitForSeconds(2.1f);
        attackCoolDown = false;

    }


    //Check if player is in the attack box collider
    public void ChildOnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && !other.isTrigger){
            other.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    
}
