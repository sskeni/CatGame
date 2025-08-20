using Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private TArray<Image> rooms = new Image[3,3];
    [SerializeField] private Image currentRoomMarker;

    public void SetCurrentRoom(int x, int y)
    {
        currentRoomMarker.rectTransform.anchoredPosition = new Vector3((x * 30) + 2.5f, (y * 30) + 2.5f, 0f); // Times 30 and plus 2.5 to account for gap between UI elements
    }

    public void SetRoomAsComplete(int x, int y)
    {
        Image completedRoom = rooms[x, y];

        completedRoom.color = Color.green;
    }
}
