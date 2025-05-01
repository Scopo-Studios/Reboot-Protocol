using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUICounter : MonoBehaviour
{
    [Header("Inventory Information")]
    public InventoryItem ammoItem; // The ammo item from inventory
    public int clipSize = 15;
    [SerializeField] private TextMeshProUGUI clipCount;
    [SerializeField] private TextMeshProUGUI ammoTotal;

    public int currentClipSize = 15;

    void Start()
    {
        UpdateAmmoUI(true);
    }

    public void FireShot()
    {
        if (currentClipSize > 0)
        {
            currentClipSize--; // Only decrease the clip size
            UpdateAmmoUI(false);    // Refresh UI
        }
        else
        {
            Debug.Log("Clip empty! Need to reload.");
        }
    }

    public void ReloadClip()
    {
        int ammoNeeded = clipSize - currentClipSize;

        if (ammoItem.numberHeld >= ammoNeeded)
        {
            // If enough ammo, fill the clip completely
            currentClipSize = clipSize;
        }
        else
        {
            // If not enough ammo, fill as much as possible
            currentClipSize += ammoItem.numberHeld;
            ammoItem.numberHeld = 0;
        }

        UpdateAmmoUI(true);
    }

    public void UpdateAmmoUI(bool empty)
{
    clipCount.text = currentClipSize.ToString();

    if (empty)
    {
        int backpackAmmo = ammoItem.numberHeld - currentClipSize;

        if (backpackAmmo <= 0)
        {
            ammoTotal.text = "0";
        }
        else
        {
            ammoTotal.text = backpackAmmo.ToString();
        }
    }
}
}
