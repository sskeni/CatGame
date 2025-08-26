using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    // Singleton References
    private static RoomHandler instance;
    public static RoomHandler Instance { get { return instance; } }

    // UI References
    [SerializeField] private TextMeshProUGUI roomClearText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private MiniMap miniMap;

    // Public References
    public int roomsCleared;
    public int enemiesKilled;

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
        enemiesKilled++;

        if (enemyCount[x, y] == 0)
        {
            StartCoroutine(ShowRoomClearText());

            miniMap.SetRoomAsComplete(x, y);
        }
        UpdateEnemyCountText();
    }

    // Shows the Room Clear UI
    private IEnumerator ShowRoomClearText()
    {
        roomsCleared++;
        roomClearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        roomClearText.gameObject.SetActive(false);

        if(roomsCleared == 9)
        {
            PlayerController.Instance.playerHealth.Die();
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
