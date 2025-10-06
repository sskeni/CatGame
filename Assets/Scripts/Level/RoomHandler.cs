using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RoomHandler : MonoBehaviour
{
    // Singleton References
    private static RoomHandler instance;
    public static RoomHandler Instance { get { return instance; } }

    // UI References
    [SerializeField] private TextMeshProUGUI roomClearText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private MiniMapUI miniMap;

    // Public Variables
    public GameObject coinPrefab;
    public int minCoinsAward;
    public int maxCoinsAward;
    public float roomDifficultyIncrease;
    
    // Public References
    [HideInInspector] public int roomsCleared;

    // Private References
    private int[,] enemyCount = new int[3, 3];
    private Tuple<int, int> currentRoomID;

    private void Awake()
    {
        CheckSingleton();
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

    // Sets the enemy count for a given room
    public void SetEnemyCount(int x, int y, int count)
    {
        enemyCount[x, y] = count;
    }

    // Lowers the enemy count for a given room by 1
    public void LowerEnemyCount(int x, int y)
    {
        enemyCount[x, y]--;
        RunStatisticsHandler.Instance.totalEnemiesKilled++;

        if (enemyCount[x, y] == 0)
        {
            StartCoroutine(ShowRoomClearText());
            StartCoroutine(SpawnCoins());

            miniMap.SetRoomAsComplete(x, y);
        }
        UpdateEnemyCountText();
    }

    // Shows the Room Clear UI
    private IEnumerator ShowRoomClearText()
    {
        DifficultyHandler.Instance.difficulty += roomDifficultyIncrease;

        roomsCleared++;
        RunStatisticsHandler.Instance.totalRoomsCleared++;
        roomClearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        roomClearText.gameObject.SetActive(false);

        if(roomsCleared == 9)
        {
            //ReloadRooms();
            CountdownUI.Instance.OpenUI();
        }
    }

    // Sets difficulty and reloads the current scene to get a new set of levels
    // Context menu for debugging purposes
    [ContextMenu("Reload Rooms")]
    public void ReloadRooms()
    {
        RunStatisticsHandler.Instance.totalHousesCleared++;
        DifficultyHandler.Instance.difficulty = Mathf.Ceil(DifficultyHandler.Instance.difficulty);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Spawns a random amount of coins on room clear
    private IEnumerator SpawnCoins()
    {
        int coinCount = Random.Range(minCoinsAward, maxCoinsAward);
        for (int i = 0; i <= coinCount; i++)
        {
            Instantiate(coinPrefab, PlayerController.Instance.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Sets the current room that the player is in and updates UI
    public void SetCurrentRoomID(Tuple<int, int> ID)
    {
        currentRoomID = ID;
        UpdateEnemyCountText();
        miniMap.SetCurrentRoom(ID.Item1, ID.Item2);
    }

    // Updates the enemy count UI
    private void UpdateEnemyCountText()
    {
        int currentEnemyCount = enemyCount[currentRoomID.Item1, currentRoomID.Item2];
        enemyCountText.text = currentEnemyCount + " enemies left";
        if (currentEnemyCount <= 0)
        {
            enemyCountText.color = Color.green;
        }
        else
        {
            enemyCountText.color = Color.white;
        }
    }
}
