using UnityEngine;
using UnityEngine.SceneManagement;  // For reloading the scene

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;  // Drag the Game Over UI Canvas here
    public GameObject ammoUI;
    public static bool isPaused = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with the plane (the player should have a tag 'Player')
        if (other.CompareTag("Player"))
        {
            // Show the Game Over screen
            ShowGameOverScreen();
        }
    }

    private void ShowGameOverScreen()
    {
        // Activate the Game Over UI (Canvas)
        if (gameOverScreen != null)
        {
            ammoUI.SetActive(!ammoUI.activeSelf);
            gameOverScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Time.timeScale = 0;  // Pauses the game
    }
}