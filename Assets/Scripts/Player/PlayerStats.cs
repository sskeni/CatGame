using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Singleton References
    private static PlayerStats instance;
    public static PlayerStats Instance { get { return instance; } }

    // Player Stats
    public float speed = 35f;
    public float jumpForce = 650f;
    public int maxJumps = 1;

    public float attackDamage = 1f;
    public float attackCooldown = 1f;
    public float critChance = 10f;
    public float critDamage = 50f;
    public int maxAttacks = 1;

    public float maxHealth = 10f;
    public float regenDelay = 5f;
    public float regenRate = 1f;
    public float damageIFrames = 1.5f;
    public float knockbackVelocity = 15f;
    
    // Base Stats
    public float baseAttackDamage { get; private set; }
    public float baseSpeed { get; private set; }

    public float baseAttackCooldown { get; private set; }
    public float baseCritChance { get; private set; }
    public float baseCritDamage { get; private set; }

    public float baseMaxHealth { get; private set; }
    public float baseRegenDelay { get; private set; }
    public float baseRegenRate { get; private set; }

    private void Awake()
    {
        CheckSingleton();

        // Get base stats
        baseAttackDamage = attackDamage;
        baseSpeed = speed;
        baseAttackCooldown = attackCooldown;
        baseCritChance = critChance;
        baseCritDamage = critDamage;
        baseMaxHealth = maxHealth;
        baseRegenDelay = regenDelay;
        baseRegenRate = regenRate;
    }

    // Sets up singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
