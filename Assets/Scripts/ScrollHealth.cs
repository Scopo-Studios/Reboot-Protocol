using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollHealth : MonoBehaviour
{
    private Image m_Image;
    private Sprite h_Status;

    [SerializeField] float scrollSpeed;
    [SerializeField] Sprite[] healthStatus;
    [SerializeField] GameObject Status;
    [SerializeField] GameObject playerObj; // Drag Player GameObject into this field

    private PlayerController player;

    void Start()
    {
        m_Image = GetComponent<Image>();

        // Clone the material to allow independent offset changes
        m_Image.material = new Material(m_Image.material);

        m_Image.color = new Color32(0, 255, 0, 255);

        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerController>();
        }

        if (healthStatus.Length > 0)
        {
            h_Status = healthStatus[0];
            Status.GetComponent<Image>().sprite = h_Status;
        }
    }

    void Update()
    {
        if (player != null)
        {
            UpdateHealthVisual(player.health);
        }

        // Animate scroll offset horizontally
        m_Image.material.mainTextureOffset += new Vector2(-scrollSpeed * Time.deltaTime, 0f);
    }


    void UpdateHealthVisual(int health)
    {
        if (health >= 70)
        {
            m_Image.color = new Color32(0, 255, 0, 255); // Green
            h_Status = healthStatus[0];
        }
        else if (health >= 40)
        {
            m_Image.color = new Color32(255, 255, 0, 255); // Yellow
            h_Status = healthStatus[1];
        }
        else
        {
            m_Image.color = new Color32(255, 0, 0, 255); // Red
            h_Status = healthStatus[2];
        }

        Status.GetComponent<Image>().sprite = h_Status;
    }
}
