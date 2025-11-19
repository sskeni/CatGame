using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractableSpawner: MonoBehaviour
{
    public GameObject chestPrefab;
    public GameObject healingFountainPrefab;
    public int minChests;
    public int maxChests;
    public int minHealingFountains;
    public int maxHealingFountains;

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
            SpawnAllInteractables();
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
    private void SpawnAllInteractables()
    {
        spawned = true;

        int chestCount = Random.Range(minChests, maxChests + 1);
        int healingFountainCount = Random.Range(minHealingFountains, maxHealingFountains + 1);
        int totalInteractableCount = chestCount + healingFountainCount;

        // just in case
        if (totalInteractableCount > spawnPoints.Count)
        {
            chestCount = spawnPoints.Count - 1;
            healingFountainCount = 1;
            totalInteractableCount = chestCount + healingFountainCount;
        }

        // min value of 2 to guarantee one chest and one healing fountain
        int[] randomIndexes = RandomSpawnIDs(totalInteractableCount, spawnPoints.Count);

        // Spawn all the chests
        for (int i = 0; i < chestCount; i++)
        {
            SpawnChest(spawnPoints[randomIndexes[i]].transform);
        }

        // Continue where we left off, spawn all the healing fountains
        for (int i = chestCount; i < randomIndexes.Length; i++)
        {
            SpawnHealingFountain(spawnPoints[randomIndexes[i]].transform);
        }
    }

    // Spawns a chest at a transform
    private void SpawnChest(Transform spawnLocation)
    {
        // Move the position down by 0.63f to put it on the ground
        float groundOffset = -0.63f;
        Vector3 spawnPosition = new Vector3(spawnLocation.position.x, spawnLocation.position.y + groundOffset, spawnLocation.position.z);
        Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
    }

    private void SpawnHealingFountain(Transform spawnLocation)
    {
        Vector3 spawnPosition = spawnLocation.position;
        Instantiate(healingFountainPrefab, spawnPosition, Quaternion.identity);
    }

    // Gives a set of unique random numbers within a range
    private int[] RandomSpawnIDs(int count, int maxExclusive)
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
