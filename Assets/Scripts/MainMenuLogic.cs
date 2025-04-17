using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    [Header("Canvas References")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject extrasMenu;
    public GameObject loading;

    [Header("Audio")]
    public AudioSource buttonSound;

    void Start()
    {
        mainMenu.GetComponent<Canvas>().enabled = true;
        optionsMenu.GetComponent<Canvas>().enabled = false;
        extrasMenu.GetComponent<Canvas>().enabled = false;
        loading.GetComponent<Canvas>().enabled = false;
    }

    public void StartButton()
    {
        if (buttonSound != null) buttonSound.Play();
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        loading.GetComponent<Canvas>().enabled = true;
        mainMenu.GetComponent<Canvas>().enabled = false;
        yield return new WaitForSeconds(1f); // Optional loading delay
        SceneManager.LoadScene("ForestTutorial");
    }

    public void OptionsButton()
    {
        if (buttonSound != null) buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        optionsMenu.GetComponent<Canvas>().enabled = true;
    }

    public void ExtrasButton()
    {
        if (buttonSound != null) buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        extrasMenu.GetComponent<Canvas>().enabled = true;
    }

    public void ExitGameButton()
    {
        if (buttonSound != null) buttonSound.Play();
        Application.Quit();
        Debug.Log("App Has Exited");
    }

    public void ReturnToMainMenuButton()
    {
        if (buttonSound != null) buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = true;
        optionsMenu.GetComponent<Canvas>().enabled = false;
        extrasMenu.GetComponent<Canvas>().enabled = false;
    }
}
