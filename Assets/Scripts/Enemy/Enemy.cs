using FirstGearGames.SmoothCameraShaker;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamageable
{
    // Enemy Stats
    [SerializeField] public float maxHealth;
    [SerializeField] private float experienceAmount;
    [SerializeField] private float healthVariance;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayer;

    // Enemy Prefabs
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private DamageNumber damageNumberPrefab;
    [SerializeField] private GameObject experienceOrbParticleSystemPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private ShakeData critShake;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private float damageVolume;
    [SerializeField] private float damagePitchRange;
    [SerializeField] private AudioClip damageCritSoundClip;
    [SerializeField] private float damageCritVolume;
    [SerializeField] private float damageCritPitchRange;

    // Damage References
    public float currentHealth { get; private set; }
    public bool canTakeDamage { get; set; } = true;
    private bool isDying;

    // Room ID
    private Tuple<int, int> roomID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        SetUpHealth();
    }

    private void SetUpHealth()
    {
        maxHealth *= DifficultyHandler.Instance.difficulty;
        //maxHealth *= Random.Range(1f - healthVariance, 1f);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);
    }

    // Damages the enemy
    public void Damage(float damageAmount, bool wasCrit)
    {
        canTakeDamage = false;
        currentHealth -= damageAmount;
        SpawnDamageNumber(damageAmount, wasCrit);
        healthBar.SetCurrentHealth(currentHealth);

        if (wasCrit) SoundFXHandler.Instance.PlaySoundFXClip(damageCritSoundClip, transform, damageCritVolume, damageCritPitchRange, damageCritPitchRange); 
        else SoundFXHandler.Instance.PlaySoundFXClip(damageSoundClip, transform, damageVolume, damagePitchRange, damagePitchRange);

        if (wasCrit) CameraShakerHandler.Shake(critShake);
        if (currentHealth <= 0) Die();
    }

    // Spawns the damage number
    private void SpawnDamageNumber(float damageAmount, bool wasCrit)
    {
        DamageNumber damageNumber = Instantiate(damageNumberPrefab);
        //damageNumber.transform.position = transform.position;
        float xOffset = Random.Range(0f, 0.5f);
        float yOffset = Random.Range(0f, 0.5f);
        damageNumber.transform.position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
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

    public bool isGrounded()
    {
        return Physics2D.Raycast(transform.position, -transform.up, groundCheckDistance, groundLayer);
    }
}
