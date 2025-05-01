using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningLight : MonoBehaviour
{
    private float spinAngle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spinAngle += 90f * Time.deltaTime;
        Vector3 currentRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentRotation.x, spinAngle, currentRotation.z);
    }
}
