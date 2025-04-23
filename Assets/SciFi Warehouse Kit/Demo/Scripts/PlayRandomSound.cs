 using UnityEngine;
 using System.Collections;
 
 public class PlayRandomSound : MonoBehaviour {
 
     public AudioSource randomSound;
     public AudioClip[] audioSources;

     public float minDelay = 3f;
    public float maxDelay = 7f;
 
     // Use this for initialization
     void Start () {
        randomSound = GetComponent<AudioSource>();
        
        StartCoroutine(PlayRandomClip());
    }

    IEnumerator PlayRandomClip()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            AudioClip clip = audioSources[Random.Range(0, audioSources.Length)];
            randomSound.PlayOneShot(clip, 0.5f);
        }
    }
 }