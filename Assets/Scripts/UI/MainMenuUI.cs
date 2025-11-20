using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        if (PersistentObjectCleaner.Instance != null) PersistentObjectCleaner.Instance.CleanObjects();
    }

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
