using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyTypes;
    private List<GameObject> spawnPoints = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();
    private bool spawnedEnemies = false;
    private Tuple<int, int> roomID;

    private void Awake()
    {
        GetAllSpawnPoints();
    }

    private void GetAllSpawnPoints()
    {
        int childNumber = transform.childCount;
        for (int i = 0; i < childNumber; i++)
        {
            spawnPoints.Add(transform.GetChild(i).gameObject);
        }
    }

    private void SpawnAllEnemies()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            //GameObject enemy = Instantiate(enemyType, spawnPoint.transform.position, Quaternion.identity);
            GameObject enemy = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length)], spawnPoint.transform.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetRoomID(roomID);
            enemies.Add(enemy);
        }

        RoomHandler.Instance.SetEnemyCount(roomID.Item1, roomID.Item2, enemies.Count);
    }

    private void DespawnAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i]);
            }
        }
        enemies.Clear();
    }

    public void SetRoomID(Tuple<int, int> ID)
    {
        roomID = ID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!spawnedEnemies)
            {
                SpawnAllEnemies();
                spawnedEnemies = true;
            }
            RoomHandler.Instance.SetCurrentRoomID(roomID);
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            //DespawnAllEnemies();
        }
    }*/
}
