using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestSpawner : MonoBehaviour
{
    public GameObject chestPrefab;
    public int maxChests;

    // Private References
    private List<GameObject> spawnPoints = new List<GameObject>();
    private bool spawned = false;

    private void Awake()
    {
        GetAllSpawnPoints();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !spawned)
        {
            SpawnAllChests();
        }
    }

    // Gets all the spawn points for chests
    private void GetAllSpawnPoints()
    {
        int childNumber = transform.childCount;
        for (int i = 0; i < childNumber; i++)
        {
            spawnPoints.Add(transform.GetChild(i).gameObject);
        }
    }

    // Spawns random amount of chests up to maxChests at random spawn points
    private void SpawnAllChests()
    {
        spawned = true;
        int[] randomIndexes = RandomChestIDs(Random.Range(1, maxChests), spawnPoints.Count);
        for (int i = 0; i < randomIndexes.Length; i++)
        {
            SpawnChest(spawnPoints[randomIndexes[i]].transform);
        }
    }

    // Spawns a chest at a transform
    private void SpawnChest(Transform spawnLocation)
    {
        // Move the position down by 0.63f to put it on the ground
        Vector3 spawnPosition = new Vector3(spawnLocation.position.x, spawnLocation.position.y - 0.63f, spawnLocation.position.z);
        Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
    }

    // Gives a set of unique random numbers within a range
    private int[] RandomChestIDs(int count, int maxExclusive)
    {
        if (count > maxExclusive)
        {
            throw new ArgumentException("count was more than maxExclusive");
        }

        int[] results = new int[count];

        // Generate a list of all numbers to maxExclusive
        List<int> numbers = new List<int>();
        for (int i = 0; i < maxExclusive; i++)
        {
            numbers.Add(i);
        }

        // Save one of the numbers from the list at random, then remove it from the list
        for (int i = 0; i < results.Length; i++)
        {
            int randomIndex = Random.Range(0, numbers.Count);
            results[i] = numbers[randomIndex];
            numbers.RemoveAt(randomIndex);
        }

        return results;
    }
}
