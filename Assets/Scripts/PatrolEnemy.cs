using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{

    public Transform target;
    public float speed = 3f;
    public float chaseRadius = 10f;
    public float attackRadius = 1.5f;
    public Rigidbody myRigidbody;
    private bool alerted;
    private bool chasing;
    private bool hit;
    private bool raged;
    private bool attacking;
    private Animator animator;
    private float rotationSpeed = 10f;
    public float health;
    private bool dead = false;
    private bool alertedByHit = false;

    public bool attackCoolDown = false;
    

    public AudioClip hurtSound;
    public AudioClip rageSound;
    public AudioClip footStepSound;
    public float footStepDelay;
    private float nextFootstep = 0;

    private bool hitAnim = true;
    private bool attackAnim = true;
    public Transform[] path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;

    private int direction;

    // Start is called before the first frame update
    void Start()
    {
    
        myRigidbody = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").transform;
        alerted = false;
        chasing = false;
        attacking = false;
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }
        currentGoal = path[0];
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius){
            alertedByHit = false;
            alerted = true;
        }
        if (Vector3.Distance(target.position, transform.position) <= attackRadius){
            if (!attacking && !attackCoolDown){
                
                StartCoroutine(Attack());
                StartCoroutine(AttackCoolDown());
                Debug.Log("attacked");
            }
        }
        if (alerted){
            speed = 8f;
            if (!chasing){
                if (!raged){
                    StartCoroutine(Rage());
                    raged = true;
                }
            }
            else {
                AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
                bool isAttackingAnim = animState.IsName("attack1LSpike") || animState.IsName("attack5");
                if (!hit && !dead && !attacking && !isAttackingAnim){
                    Chase();
                }
                
            }
            
        }
        else {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance){
                
                footStepDelay = 0.8f;
                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, speed * Time.deltaTime);
                Vector3 direction = currentGoal.position - transform.position;
                direction.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                myRigidbody.MovePosition(temp);
                nextFootstep -= Time.deltaTime;
                if (nextFootstep <= 0) 
                {
                    GetComponent<AudioSource>().PlayOneShot(footStepSound, 2f);
                    nextFootstep += footStepDelay;
                }
                
            }
            else {
                ChangeGoal();
            }
        }
    }

    void Chase(){
        footStepDelay = 0.3f;
        Vector3 temp = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; 
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        myRigidbody.MovePosition(temp);
        nextFootstep -= Time.deltaTime;
        if (nextFootstep <= 0) 
        {
            GetComponent<AudioSource>().PlayOneShot(footStepSound, 2f);
            nextFootstep += footStepDelay;
        }
        
    }

    private IEnumerator Rage(){
        GetComponent<AudioSource>().PlayOneShot(rageSound, 0.3f);
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; 
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (!alertedByHit) animator.SetBool("Alerted", true);
        yield return new WaitForSeconds(2f);
        if (alertedByHit) animator.SetBool("Alerted", true);
        chasing = true;
    }

    private void ChangeGoal(){
        if (currentPoint == path.Length - 1 || currentPoint == 0){
            direction *= -1;
            currentPoint += direction;
            currentGoal = path[currentPoint];
        }
        else {
            currentPoint += direction;
            currentGoal = path[currentPoint];
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
    private IEnumerator Hit(){
        hit = true;
        if (hitAnim){
            animator.SetTrigger("Hit");
            yield return new WaitForSeconds(0.7f);
            hitAnim = false;
        }
        else {
            animator.SetTrigger("Hit2");
            yield return new WaitForSeconds(0.7f);
            hitAnim = true;
        }
        hit = false;
        if (!alerted){
            alerted = true;
            alertedByHit = true;
        } 
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
        // Choose animation
        if (attackAnim)
        {
            animator.SetTrigger("Attack");
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
            attackAnim = false;
        }
        else
        {
            animator.SetTrigger("Attack2");
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
            attackAnim = true;
        }
        animator.SetBool("Attacking", false);
        attacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        attackCoolDown = true;
        yield return new WaitForSeconds(1.5f);
        attackCoolDown = false;

    }


    //Check if player is in the attack box collider
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && !other.isTrigger){
            other.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    
}
