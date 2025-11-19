using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    // UI References
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI healthRegenRateText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI critRateText;
    public TextMeshProUGUI critDamageText;
    public TextMeshProUGUI housesClearedText;
    public TextMeshProUGUI roomsClearedText;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI coinCountText;
    public TextMeshProUGUI chestCountText;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Opens the end screen and sets all the texts
    public void OpenUI()
    {
        // Format time
        float timeStopped = Time.time;
        float totalTimeInSeconds = timeStopped - RunStatisticsHandler.Instance.runStartTime;
        float hours = TimeSpan.FromSeconds(totalTimeInSeconds).Hours;
        float minutes = TimeSpan.FromSeconds(totalTimeInSeconds).Minutes;
        float seconds = TimeSpan.FromSeconds(totalTimeInSeconds).Seconds;

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        levelText.text = "Level " + PlayerLevel.Instance.level;
        healthText.text = "Health: " + PlayerStats.Instance.maxHealth;
        healthRegenRateText.text = "Health Regen Rate: " + PlayerStats.Instance.healthRegen;
        attackText.text = "Attack: " + (PlayerStats.Instance.attackDamage * PlayerStats.Instance.attackMultiplier);
        critRateText.text = "Critical Rate: " + PlayerStats.Instance.critChance + "%";
        critDamageText.text = "Critical Damage: " + PlayerStats.Instance.critDamage + "%";

        housesClearedText.text = "Houses Cleared: " + RunStatisticsHandler.Instance.totalHousesCleared;
        roomsClearedText.text = "Rooms Cleared: " + RunStatisticsHandler.Instance.totalRoomsCleared;
        enemiesKilledText.text = "Enemies Killed: " + RunStatisticsHandler.Instance.totalEnemiesKilled;
        coinCountText.text = "x " + RunStatisticsHandler.Instance.totalCoinsCollected;
        chestCountText.text = "x " + RunStatisticsHandler.Instance.totalChestsOpened;
        gameObject.SetActive(true);
    }

    // Restarts the game
    public void Restart()
    {
        PersistentObjectCleaner.Instance.CleanObjects();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
