using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject lid;
    private bool inRange;
    private bool alreadyOpened;
    private bool pickedUp;
    public GameObject player;
    private PlayerController playerControl;
    public GameObject prefabToInstantiate;

    void Start(){
        alreadyOpened = false;
        inRange = false;
        playerControl = player.GetComponent<PlayerController>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.F) && inRange && !alreadyOpened){
            StartCoroutine(Open());
            alreadyOpened = true;
            playerControl.pickUp = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && inRange && alreadyOpened && !pickedUp){
            StartCoroutine(pickUp());
            pickedUp = true;
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && !other.isTrigger){
            
            if (playerControl != null){
                if (!alreadyOpened){
                    playerControl.use = true;
                }
                else if (alreadyOpened && !pickedUp) {
                    playerControl.use = false;
                    playerControl.pickUp = true;
                }
                else {
                    playerControl.use = false;
                    playerControl.pickUp = false;
                }
                
            }
            inRange = true;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("Player") && !other.isTrigger){
            
            if (playerControl != null){
                playerControl.use = false;
                playerControl.pickUp = false;
            }
            inRange = false;
        }
    }

    private IEnumerator Open(){
        yield return new WaitForSeconds(1.4f);
        Instantiate(prefabToInstantiate, transform);
        lid.transform.localRotation = Quaternion.Euler(-135, 0, 0);
        playerControl.use = false;
    }

    private IEnumerator pickUp(){
        yield return new WaitForSeconds(1.2f);
        GameObject.Destroy(transform.Find("MedKit(Clone)").gameObject);
        playerControl.pickUp = false;
    }
}
