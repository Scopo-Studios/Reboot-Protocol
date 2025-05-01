using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject gameOverScreen; 
    public GameObject loadingCanvas;
    public string mainMenuSceneName = "MainMenu";

    public void setUp()
    {
        gameObject.SetActive(true);
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


    public void Quit()
    {
        Debug.Log("Game Ended");
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}