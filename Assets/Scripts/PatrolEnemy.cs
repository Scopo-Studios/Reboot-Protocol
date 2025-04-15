using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{

    public Transform target;
    private float speed = 3f;
    public float chaseRadius = 10f;
    public Rigidbody myRigidbody;
    private bool alerted;
    private bool chasing;
    private bool hit;
    private Animator animator;
    private float rotationSpeed = 10f;

    public Transform[] path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").transform;
        alerted = false;
        chasing = false;
        animator = GetComponent<Animator>();
        currentGoal = path[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius){
            alerted = true;
        }
        if (alerted){
            speed = 3f;
            if (!chasing){
                StartCoroutine(Rage());
            }
            else {
                if (!hit){
                    Chase();
                }
            }
            
        }
        else {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance){
                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, speed * Time.deltaTime);
                Vector3 direction = currentGoal.position - transform.position;
                direction.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                myRigidbody.MovePosition(temp);
            }
            else {
                ChangeGoal();
            }
        }
    }

    void Chase(){
        Vector3 temp = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; 
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        myRigidbody.MovePosition(temp);
    }

    private IEnumerator Rage(){
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; 
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        animator.SetBool("Alerted", true);
        yield return new WaitForSeconds(2f);
        chasing = true;
    }

    private void ChangeGoal(){
        if (currentPoint == path.Length - 1){
            currentPoint = 0;
            currentGoal = path[0];
        }
        else {
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }

    public void TakeDamage(){
        
        StartCoroutine(Hit());
    }
    private IEnumerator Hit(){
        hit = true;
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(1.4f);
        hit = false;
    }
}
