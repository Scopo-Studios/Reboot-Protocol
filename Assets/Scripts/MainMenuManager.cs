using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the next scene (your game scene must be added in Build Settings)
        SceneManager.LoadScene("ForestTutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit"); // Won't show unless in built version
    }
}
