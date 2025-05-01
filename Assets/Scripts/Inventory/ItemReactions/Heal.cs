using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private PlayerController playerControl; // Reference to the PlayerController
    public InventoryItem healItem; // The item required to heal (dragged into the inspector)
    public InventoryManager inventoryManager; // Reference to the InventoryManager for UI updates
    public int healAmount = 20; // Amount of health to heal

    // Start is called before the first frame update
    void Start()
    {
        // Get the PlayerController component attached to the same GameObject (character model)
        playerControl = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        // Check for the "C" key press to trigger healing
        if (Input.GetKeyDown(KeyCode.Z) && playerControl != null && inventoryManager != null)
        {
            // Check if the player has the healing item
            if (healItem.numberHeld > 0)
            {
                // Heal the player
                playerControl.Heal(healAmount);
                healItem.Use();

                //clear all of the inventory slots
                inventoryManager.ClearInventorySlots();
                //Refill all slots with new numbers
                inventoryManager.MakeInventorySlots();
                inventoryManager.SetTextAndButton("", false);

                Debug.Log("Healed! Item removed. Remaining: " + healItem.numberHeld);
            }
            else
            {
                Debug.Log("Not enough healing items to use.");
            }
        }
    }
}