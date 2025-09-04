using Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    // UI References
    [SerializeField] private TArray<Image> rooms = new Image[3,3];
    [SerializeField] private Image currentRoomMarker;
    [SerializeField] private Color completedRoomColor;

    // Sets the cursor to the position of a given room
    public void SetCurrentRoom(int x, int y)
    {
        currentRoomMarker.rectTransform.anchoredPosition = new Vector3((x * 30) + 2.5f, (y * 30) + 2.5f, 0f); // Times 30 and plus 2.5 to account for gap between UI elements
    }

    // Marks the UI of a given room as complete
    public void SetRoomAsComplete(int x, int y)
    {
        Image completedRoom = rooms[x, y];

        completedRoom.color = completedRoomColor;
    }
}
