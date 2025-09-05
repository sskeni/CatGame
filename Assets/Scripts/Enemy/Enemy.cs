using System;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void OnEnemyDamagedEventHandler(float currentHealth);

public class Enemy : MonoBehaviour, IDamageable
{
    // Enemy Stats
    [SerializeField] public float maxHealth;
    [SerializeField] private float experienceAmount;

    // Enemy Prefabs
    [SerializeField] private DamageNumber damageNumberPrefab;
    [SerializeField] private GameObject experienceOrbParticleSystemPrefab;
    [SerializeField] private GameObject coinPrefab;

    // Damage References
    public float currentHealth { get; private set; }
    public bool hasTakenDamage { get; set; }
    private bool isDying;

    // Events
    public event OnEnemyDamagedEventHandler onEnemyDamaged;

    // Room ID
    private Tuple<int, int> roomID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentHealth = maxHealth;
        onEnemyDamaged?.Invoke(currentHealth);
    }

    // Damages the enemy
    public void Damage(float damageAmount, bool wasCrit)
    {
        hasTakenDamage = true;
        currentHealth -= damageAmount;
        SpawnDamageNumber(damageAmount, wasCrit);
        onEnemyDamaged?.Invoke(currentHealth);

        if (currentHealth <= 0) Die();
    }

    // Spawns the damage number
    private void SpawnDamageNumber(float damageAmount, bool wasCrit)
    {
        DamageNumber damageNumber = Instantiate(damageNumberPrefab);
        //damageNumber.transform.position = transform.position;
        damageNumber.transform.position = new Vector3(transform.position.x + Random.Range(0f, 0.5f), transform.position.y + Random.Range(0f, 0.5f), transform.position.z);
        damageNumber.DamageWasCrit(wasCrit);
        damageNumber.SetDamageAmount(damageAmount);
    }

    // Sets the Room ID of the enemy
    public void SetRoomID(Tuple<int, int> ID)
    {
        roomID = ID;
    }

    // Kills the enemy
    private void Die()
    {
        if (!isDying)
        {
            isDying = true;
            SpawnExperienceOrbs();
            SpawnCoins();
            RoomHandler.Instance.LowerEnemyCount(roomID.Item1, roomID.Item2);
            Destroy(gameObject);
        }
    }

    // Spawns experience orbs by instantiating a particle system
    private void SpawnExperienceOrbs()
    {
        if (experienceOrbParticleSystemPrefab != null && PlayerController.Instance != null)
        {
            GameObject p = Instantiate(experienceOrbParticleSystemPrefab);
            p.transform.position = this.transform.position;
            p.GetComponent<ExperienceOrbParticleCollision>().experienceAmount = experienceAmount;
            p.GetComponent<ParticleSystem>().trigger.AddCollider(PlayerController.Instance.gameObject.GetComponent<BoxCollider2D>());
            p.gameObject.SetActive(true);
        }
    }

    // Spawns coins by instantiating a random amount of game objects
    private void SpawnCoins()
    {
        int coinNum = Random.Range(0, 4);
        for (int i = 0; i <= coinNum; i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }
}
