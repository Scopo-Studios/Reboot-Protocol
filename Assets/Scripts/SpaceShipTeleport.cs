using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShipTeleport : MonoBehaviour
{

    public AudioClip engineSound;
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && !other.isTrigger){
            StartCoroutine(Teleport());
        }
    }
    private IEnumerator Teleport()
    {
        GetComponent<AudioSource>().PlayOneShot(engineSound, 7f);
        yield return new WaitForSeconds(3.8f);
        
        SceneManager.LoadScene("SpaceshipLevel");
    }
}
