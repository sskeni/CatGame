using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenMenu : MonoBehaviour
{
    // Singleton References
    private static EndScreenMenu instance;
    public static EndScreenMenu Instance { get { return instance; } }

    // UI References
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI roomsClearedText;
    public TextMeshProUGUI enemiesKilledText;

    // Private References
    private float timeStarted;

    void Awake()
    {
        CheckSingleton();
        timeStarted = Time.time;
        gameObject.SetActive(false);
    }

    // Sets up singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Opens the end screen and sets all the texts
    public void OpenUI()
    {
        // Format time
        float timeStopped = Time.time;
        float totalTimeInSeconds = timeStopped - timeStarted;
        float minutes = TimeSpan.FromSeconds(totalTimeInSeconds).Minutes;
        float seconds = TimeSpan.FromSeconds(totalTimeInSeconds).Seconds;

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        levelText.text = "Level " + PlayerController.Instance.playerLevel.level;
        attackText.text = "Attack: " + PlayerController.Instance.attackDamage;
        healthText.text = "Health: " + PlayerController.Instance.playerHealth.maxHealth;
        roomsClearedText.text = "Rooms Cleared: " + RoomHandler.Instance.roomsCleared;
        enemiesKilledText.text = "Enemies Killed: " + RoomHandler.Instance.enemiesKilled;
        gameObject.SetActive(true);
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

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
