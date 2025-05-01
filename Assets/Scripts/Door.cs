using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioSource sound;
    public GameObject player;
    private PlayerController playerControl;
    private Animator animator;
    private bool inRange;
    private bool openClose;
    public AudioClip openSound;
    public AudioClip closeSound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        inRange = false;
        playerControl = player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        openClose = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inRange){
            if (!openClose){
                StartCoroutine(Open());
            }
            else {
                StartCoroutine(Close());
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && !other.isTrigger){
            if (playerControl != null){    
                playerControl.use = true;
            }
            inRange = true;
            transform.Find("Pop-up").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("Player") && !other.isTrigger){
            
            if (playerControl != null){
                playerControl.use = false;
            }
            inRange = false;
            transform.Find("Pop-up").gameObject.SetActive(false);
        }
    }

    private IEnumerator Open(){
        yield return new WaitForSeconds(1.4f);
        
        animator.SetBool("OpenClose", true);
        openClose = true;
        
        
        sound.PlayOneShot(openSound, 0.5f);
    }
    private IEnumerator Close(){
        yield return new WaitForSeconds(1.4f);
        
        animator.SetBool("OpenClose", false);
        openClose = false;
        yield return new WaitForSeconds(0.2f);
        sound.PlayOneShot(closeSound, 0.3f);
    }
}
