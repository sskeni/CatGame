using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    // Player Stats
    public float maxHealth = 10f;
    public float regenDelay = 5f;
    public float regenRate = 1f;

    // Base Stats
    public float baseMaxHealth { get; private set; }
    public float baseRegenDelay { get; private set; }
    public float baseRegenRate { get; private set; }

    // Backend Serialized References
    [SerializeField] private float damageIFrames = 1.5f;
    [SerializeField] private float knockbackVelocity = 15f;
    [SerializeField] private DamageNumber damageNumberPrefab;

    // Player Reference
    private PlayerController playerController;

    // Damage Variables
    public float currentHealth { get; private set; }
    public bool hasTakenDamage { get; set; }
    private bool isRegeningHealth = false;
    private Coroutine regenCoroutine;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        currentHealth = maxHealth;
        baseMaxHealth = maxHealth;
        baseRegenDelay = regenDelay;
        baseRegenRate = regenRate;
    }

    // Sets max health
    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth; // fully heals when setting max health
    }

    // Damage with knockback
    public void DamageWithKnockback(float damageAmount, Vector3 otherPostiion)
    {
        if (hasTakenDamage == false)
        {
            Vector2 knockbackDirection = transform.position - otherPostiion;
            knockbackDirection = knockbackDirection.normalized;
            playerController.rb.linearVelocity = knockbackDirection * knockbackVelocity;
            Damage(damageAmount, false); // false bc enemies cannot crit
        }
    }

    // Damage the player
    public void Damage(float damageAmount, bool wasCrit)
    {
        if (hasTakenDamage == false)
        {
            hasTakenDamage = true;
            currentHealth -= damageAmount;
            SpawnDamageNumber(damageAmount);
            StartCoroutine(DoDamageAnimation());

            // If currently regening, stop
            if (regenCoroutine != null) StopCoroutine(regenCoroutine);
            regenCoroutine = StartCoroutine(DoRegeneration());
        }
    }

    // Damage animation coroutine
    private IEnumerator DoDamageAnimation()
    {
        float damageTimer = 0f;

        while (damageTimer < damageIFrames)
        {
            playerController.sprite.enabled = !playerController.sprite.enabled;
            damageTimer += Time.fixedDeltaTime;
            damageTimer += 0.1f; // Account for the WaitForSeconds
            yield return new WaitForSeconds(0.1f);
        }

        hasTakenDamage = false; // Let the Player be damageable again
        playerController.sprite.enabled = true; // Make sure sprite is visible at the end
    }

    // Spawn damage number
    private void SpawnDamageNumber(float damageAmount)
    {
        DamageNumber damageNumber = Instantiate(damageNumberPrefab);
        damageNumber.transform.position = transform.position;
        damageNumber.SetDamageAmount(damageAmount);
    }

    // Health regeneration coroutine
    private IEnumerator DoRegeneration()
    {
        yield return new WaitForSeconds(regenDelay);
        isRegeningHealth = true;
        while (isRegeningHealth)
        {
            isRegeningHealth = Heal(regenRate);
            yield return new WaitForSeconds(0.25f);
        }
    }

    // Returns false if max health is reached
    public bool Heal(float healAmount)
    {
        currentHealth += healAmount;
        if  (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            return false;
        }
        return true;
    }
}
