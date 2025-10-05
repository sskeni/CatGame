using UnityEngine;

public class RunStatisticsHandler : MonoBehaviour
{
    private static RunStatisticsHandler instance;
    public static RunStatisticsHandler Instance { get { return instance; } }

    // Stats
    public int totalCoinsCollected;
    public int totalRoomsCleared;
    public int totalEnemiesKilled;
    public int totalChestsOpened;
    public int totalHousesCleared;

    private void Awake()
    {
        CheckSingleton();
    }

    // Set up singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
