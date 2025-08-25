using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Private References
    private bool isPaused;

    private void Start()
    {
        PlayerController.Instance.controls.Player.Pause.performed += ctx => PauseGame();
        PlayerController.Instance.controls.UI.Pause.performed += ctx => PauseGame();
        this.gameObject.SetActive(false);
        isPaused = false;
    }

    // Opens or closes the pause UI and freezes or unfreezes the game
    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        this.gameObject.SetActive(isPaused);
        if (isPaused)
        {
            PlayerController.Instance.DisablePlayControls();
            PlayerController.Instance.EnableUIControls();
        }
        else
        {
            PlayerController.Instance.EnablePlayControls();
            PlayerController.Instance.DisableUIControls();
        }

    }

    // Returns to the main menu
    public void MainMenu()
    {
        StartCoroutine(LoadSceneAsync());
    }

    // Loads the main menu asyncronously
    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
