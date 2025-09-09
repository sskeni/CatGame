using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Starts the game
    public void StartGame()
    {
        StartCoroutine(LoadAsyncScene());
    }

    // Closes the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Load the scene asyncronously for future loading screen
    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Gameplay");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
