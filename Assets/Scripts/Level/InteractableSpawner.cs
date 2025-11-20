using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractableSpawner: MonoBehaviour
{
    public GameObject chestPrefab;
    public int minChests;
    public int maxChests;

    public GameObject healingFountainPrefab;
    public int minHealingFountains;
    public int maxHealingFountains;

    public GameObject litterBoxPrefab;
    public int minLitterBoxes;
    public int maxLitterBoxes;

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
        int litterBoxCount = Random.Range(minLitterBoxes, maxLitterBoxes + 1);
        int totalInteractableCount = chestCount + healingFountainCount + litterBoxCount;

        // just in case
        if (totalInteractableCount > spawnPoints.Count)
        {
            chestCount = spawnPoints.Count - 1;
            healingFountainCount = 1;
            litterBoxCount = 0;
            totalInteractableCount = chestCount + healingFountainCount + litterBoxCount;
        }

        // Spawn all the chests
        for (int i = 0; i < chestCount; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            SpawnChest(spawnPoint.transform);
            spawnPoints.Remove(spawnPoint);
        }

        // Spawn all the healing fountains
        for (int i = 0; i < healingFountainCount; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            SpawnHealingFountain(spawnPoint.transform);
            spawnPoints.Remove(spawnPoint);
        }

        for (int i = 0; i < litterBoxCount; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            SpawnLitterBox(spawnPoint.transform);
            spawnPoints.Remove(spawnPoint);
        }
    }

    // Spawns a chest at a transform
    private void SpawnChest(Transform spawnTransform)
    {
        // Move the position down by 0.63f to put it on the ground
        float groundOffset = -0.63f;
        Vector3 spawnPosition = new Vector3(spawnTransform.position.x, spawnTransform.position.y + groundOffset, spawnTransform.position.z);
        Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
    }

    private void SpawnHealingFountain(Transform spawnTransform)
    {
        Instantiate(healingFountainPrefab, spawnTransform.position, Quaternion.identity);
    }

    private void SpawnLitterBox(Transform spawnTransform)
    {
        Instantiate(litterBoxPrefab, spawnTransform.position, Quaternion.identity);
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
