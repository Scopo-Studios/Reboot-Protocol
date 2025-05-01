using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool inRange = false;
    private GameObject player;
    public InventoryItem itemToAdd;  
    public PlayerInventory playerInventory;
    [SerializeField] private int amountToIncrease = 1;

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Debug.Log("Player entered pickup range.");
            inRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            inRange = false;
            player = null;
        }
    }

    void PickUpItem()
    {
        if (playerInventory && itemToAdd)
        {
            if (!playerInventory.myInventory.Contains(itemToAdd))
            {
                playerInventory.myInventory.Add(itemToAdd);
            }

            if (itemToAdd.unique)
            {
                itemToAdd.numberHeld = 1;
            }
            else
            {
                itemToAdd.numberHeld += amountToIncrease; 
            }

            AmmoUICounter ammoUI = FindObjectOfType<AmmoUICounter>();
            ammoUI.UpdateAmmoUI(true);
                
        }
    }
}