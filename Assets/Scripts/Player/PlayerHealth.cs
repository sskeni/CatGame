using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    // Prefab References
    [SerializeField] private DamageNumber damageNumberPrefab;
    public ShakeData damageShakeData;

    // Damage Variables
    public float currentHealth { get; private set; }
    public bool hasTakenDamage { get; set; }
    public bool canPassivelyRegen = true;

    private bool isRegeningHealth = false;
    private Coroutine regenCoroutine;
    private bool isDying = false;

    private void Start()
    {
        currentHealth = PlayerStats.Instance.maxHealth;
    }

    // Fully heals when setting max health
    public void SetMaxHealth(float newMaxHealth)
    {
        currentHealth = PlayerStats.Instance.maxHealth; 
    }

    // Damage with knockback
    public void DamageWithKnockback(float damageAmount, Vector3 otherPostiion)
    {
        if (hasTakenDamage == false)
        {
            Vector2 knockbackDirection = transform.position - otherPostiion;
            knockbackDirection = knockbackDirection.normalized;
            PlayerController.Instance.rb.linearVelocity = knockbackDirection * PlayerStats.Instance.knockbackVelocity;
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
            CameraShakerHandler.Shake(damageShakeData);

            // If currently regening, stop
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
            }
            if (canPassivelyRegen)
            {
                regenCoroutine = StartCoroutine(DoRegeneration());
            }

            if (currentHealth <= 0) Die();
        }
    }

    // Damage animation coroutine
    private IEnumerator DoDamageAnimation()
    {
        float damageTimer = 0f;

        while (damageTimer < PlayerStats.Instance.damageIFrames)
        {
            PlayerController.Instance.sprite.enabled = !PlayerController.Instance.sprite.enabled;
            damageTimer += Time.fixedDeltaTime;
            damageTimer += 0.1f; // Account for the WaitForSeconds
            yield return new WaitForSeconds(0.1f);
        }

        if(!isDying) hasTakenDamage = false; // Let the Player be damageable again if they are not dead
        PlayerController.Instance.sprite.enabled = true; // Make sure sprite is visible at the end
    }

    // Spawn damage number
    private void SpawnDamageNumber(float damageAmount)
    {
        DamageNumber damageNumber = Instantiate(damageNumberPrefab);
        damageNumber.transform.position = transform.position;
        damageNumber.SetDamageAmount(damageAmount);
    }

    // Spawns a heal number
    private void SpawnHealNumber(float healAmount, bool wasCrit) 
    {
        DamageNumber damageNumber = Instantiate(damageNumberPrefab);
        damageNumber.transform.position = transform.position;
        damageNumber.SetDamageAmount(healAmount);
        damageNumber.WasHeal(wasCrit);
    }

    // Health regeneration coroutine
    private IEnumerator DoRegeneration()
    {
        yield return new WaitForSeconds(PlayerStats.Instance.regenDelay);
        isRegeningHealth = true;
        while (isRegeningHealth)
        {
            isRegeningHealth = Heal(PlayerStats.Instance.healthRegen);
            yield return new WaitForSeconds(0.25f);
        }
    }

    // Returns false if max health is reached
    public bool Heal(float healAmount)
    {
        currentHealth += healAmount;

        SpawnHealNumber(healAmount, false);

        if  (currentHealth >= PlayerStats.Instance.maxHealth)
        {
            currentHealth = PlayerStats.Instance.maxHealth;
            return false;
        }
        return true;
    }

    public void Die()
    {
        isDying = true;
        PlayerController.Instance.DisablePlayControls();
        EndScreenUI.Instance.OpenUI();
    }
}
