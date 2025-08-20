using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    private static RoomHandler instance;
    public static RoomHandler Instance { get { return instance; } }

    [SerializeField] private TextMeshProUGUI roomClearText;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private MiniMap miniMap;

    private int[,] enemyCount = new int[3, 3];
    private Tuple<int, int> currentRoomID;

    private void Awake()
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

    public void SetEnemyCount(int x, int y, int count)
    {
        enemyCount[x, y] = count;
    }

    public void LowerEnemyCount(int x, int y)
    {
        enemyCount[x, y]--;

        if (enemyCount[x, y] == 0)
        {
            StartCoroutine(ShowRoomClearText());

            miniMap.SetRoomAsComplete(x, y);
        }
        UpdateEnemyCountText();
    }

    private IEnumerator ShowRoomClearText()
    {
        roomClearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        roomClearText.gameObject.SetActive(false);
    }

    public void SetCurrentRoomID(Tuple<int, int> ID)
    {
        currentRoomID = ID;
        UpdateEnemyCountText();
        miniMap.SetCurrentRoom(ID.Item1, ID.Item2);
    }

    private void UpdateEnemyCountText()
    {
        int currentEnemyCount = enemyCount[currentRoomID.Item1, currentRoomID.Item2];
        enemyCountText.text = currentEnemyCount + " enemies left";
        if (currentEnemyCount == 0)
        {
            enemyCountText.color = Color.green;
        }
        else
        {
            enemyCountText.color = Color.white;
        }
    }
}
