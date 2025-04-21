using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseGame : MonoBehaviour
{
    public GameObject menu;
    public GameObject resume;
    public GameObject quit;

    public GameObject loadingCanvas; // <-- assign this in Inspector
    public string mainMenuSceneName = "MainMenu"; // <-- set to your main menu scene name


    private bool isPaused = false;

    void Start()
    {
        menu.SetActive(false);

        if (loadingCanvas != null) loadingCanvas.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetButtonDown("pause"))
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
    }

    public void Resume()
    {
        isPaused = false;
        menu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Exit()
    {

        StartCoroutine(ExitToMainMenu());
    }

    private IEnumerator ExitToMainMenu()
    {
        // Reset player progress (example using PlayerPrefs)
        PlayerPrefs.DeleteAll(); // ← clears saved progress

        if (loadingCanvas != null) loadingCanvas.SetActive(true);
        Time.timeScale = 1f; // unpause the game before switching

        yield return new WaitForSeconds(1f); // optional delay for loading effect

        SceneManager.LoadScene(mainMenuSceneName);

    }
}
