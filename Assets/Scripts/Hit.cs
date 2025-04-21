using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public SleepEnemy parent;
    // Start is called before the first frame update
    void Start(){
        parent = GetComponentInParent<SleepEnemy>();
    }

    void OnTriggerEnter(Collider other){
        parent.ChildOnTriggerEnter(other);
    }
}
