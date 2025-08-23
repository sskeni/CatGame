using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    // Level Paramaters
    [SerializeField] private GameObject[] tiles;
    [SerializeField] private int tileSize;
    [SerializeField] private int levelWidth;

    private void Start()
    {
        InstantiateLevelTiles();
    }

    // Creates level tiles randomly
    private void InstantiateLevelTiles()
    {
        for (int i = 0; i < levelWidth; i++)
        {
            for (int j = 0; j < levelWidth; j++)
            {
                GameObject room = Instantiate(tiles[Random.Range(0, tiles.Length)], new Vector3(i * tileSize, j * tileSize, 0), Quaternion.identity);
                room.name = "Room [" + i + ", " + j + "]";
                room.transform.parent = this.transform;
                room.SetActive(true);

                // Set RoomID
                room.GetComponentInChildren<EnemySpawner>().SetRoomID(new Tuple<int, int>(i, j));
            }
        }
    }
}
