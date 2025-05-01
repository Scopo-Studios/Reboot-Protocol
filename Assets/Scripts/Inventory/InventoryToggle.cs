using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryScreen;
    public GameObject ammoUI;
    public static bool isPaused = false; // make it static so Player scripts can access it

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventoryScreen.SetActive(!inventoryScreen.activeSelf);
            ammoUI.SetActive(!ammoUI.activeSelf);
            timeStop(!isPaused);
            isPaused = !isPaused;

            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void timeStop(bool gamePause)
    {
        if (!gamePause)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}