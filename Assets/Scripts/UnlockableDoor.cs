using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockableDoor : MonoBehaviour
{
    public InventoryItem keyItem; // The specific key required
    public PlayerInventory playerInventory;
    public bool isOpen;
    public TextMeshProUGUI doorMessageText;
    private bool inRange;
    private bool openClose;
    private PlayerController playerControl;
    public GameObject gameOverPlane;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inRange)
        {
            if (!isOpen && HasKey())
            {
                OpenDoor();
            }
            else
            {
                DoorIsLocked();
            }
        }
    }

    private bool HasKey()
    {
        return playerInventory.myInventory.Contains(keyItem) && keyItem.numberHeld == 1;
    }

    public void OpenDoor()
    {
        // Remove the key from inventory
        keyItem.numberHeld -= 1;
        if (keyItem.numberHeld <= 0)
        {
            playerInventory.myInventory.Remove(keyItem);
        }

        isOpen = true;

        // Finally deactivate the door object
        gameObject.SetActive(false);

        ActivatePlane();
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

    public void DoorIsLocked()
    {
        StartCoroutine(ShowDoorMessage());
    }

    private IEnumerator ShowDoorMessage()
    {
        doorMessageText.gameObject.SetActive(true); // Show the text

        yield return new WaitForSeconds(3f); // Wait for 3 seconds

        doorMessageText.gameObject.SetActive(false); // Hide the text
    }

    private void ActivatePlane()
    {
        if (gameOverPlane != null)
        {
            gameOverPlane.SetActive(true); // Activate the plane
        }
    }
}