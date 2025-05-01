using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionUpdate : MonoBehaviour
{

    [SerializeField] private Sprite fine;
    [SerializeField] private Sprite caution;
    [SerializeField] private Sprite danger;
    [SerializeField] private Image condition;
    [SerializeField] private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        condition.sprite = fine;   
        
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();  // Find the PlayerController if not set in inspector
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.health > 69){
            condition.sprite = fine;
        }
        else if (40 <= playerController.health && playerController.health <= 69){
            condition.sprite = caution;
        }
        else {
            condition.sprite = danger;
        }
    }
}
