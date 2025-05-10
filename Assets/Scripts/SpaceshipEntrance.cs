using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipEntrance : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && !other.isTrigger){
            StartCoroutine(Teleport());
        }
    }
    private IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1.0f);
        
        SceneManager.LoadScene("SpaceshipLevel");
    }
}
