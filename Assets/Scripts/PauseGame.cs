using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject menu;
    public GameObject resume;
    public GameObject quit;

    public GameObject loadingCanvas;
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;
    private PlayerController playerController; // Reference to PlayerController

    void Start()
    {
        menu.SetActive(false);

        if (loadingCanvas != null) loadingCanvas.SetActive(false);

        Time.timeScale = 1f;

        // Find PlayerController in scene
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        menu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;

        if (playerController != null)
            playerController.canShoot = !isPaused; // Disable shooting when paused
    }

    public void Resume()
    {
        isPaused = false;
        menu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null)
            playerController.canShoot = true;
    }

    public void Exit()
    {
        StartCoroutine(ExitToMainMenu());
    }

    private IEnumerator ExitToMainMenu()
    {
        PlayerPrefs.DeleteAll();

        if (loadingCanvas != null) loadingCanvas.SetActive(true);
        Time.timeScale = 1f;

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
