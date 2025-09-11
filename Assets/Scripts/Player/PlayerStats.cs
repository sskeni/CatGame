using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Singleton References
    private static PlayerStats instance;
    public static PlayerStats Instance { get { return instance; } }

    // Player Stats
    public float moveSpeed = 35f;
    public float jumpForce = 650f;
    public int maxJumps = 1;

    public float attackDamage = 1f;
    public float attackMultiplier = 1f;
    public float attackCooldown = 1f;
    public float critChance = 10f;
    public float critDamage = 50f;
    public float lowerDamageVariance = 1f;
    public float upperDamageVariance = 1f;
    public int maxAttacks = 1;

    public float maxHealth = 10f;
    public float healthRegen = 1f;
    public float regenDelay = 5f;
    public float damageIFrames = 1.5f;
    public float knockbackVelocity = 15f;
    
    public float lifesteal = 0f;
    public float lifestealMultiplier = 1f;
    public bool lifestealDoesDamage = false;

    // Base Stats
    private float baseMoveSpeed;
    private int baseMaxJumps;

    private float baseAttackDamage;
    private float baseAttackMultiplier;
    private float baseAttackCooldown;
    private float baseCritChance;
    private float baseCritDamage;
    private float baseLowerDamageVariance;
    private float baseUpperDamageVariance;
    private int baseMaxAttacks;

    private float baseMaxHealth;
    private float baseHealthRegen;
    private float baseRegenDelay;

    private float baseLifesteal;
    private float baseLifestealMultiplier;

    // Stat increase lists
    private List<StatChange> moveSpeedChange = new List<StatChange>();
    private List<StatChange> maxJumpChange = new List<StatChange>();

    private List<StatChange> attackChange = new List<StatChange>();
    private List<StatChange> attackMultiplierChange = new List<StatChange>();
    private List<StatChange> attackCooldownChange = new List<StatChange>();
    private List<StatChange> critChanceChange = new List<StatChange>();
    private List<StatChange> critDamageChange = new List<StatChange>();
    private List<StatChange> lowerDamageVarianceChange = new List<StatChange>();
    private List<StatChange> upperDamageVarianceChange = new List<StatChange>();
    private List<StatChange> maxAttackChange = new List<StatChange>();

    private List<StatChange> maxHealthChange = new List<StatChange>();
    private List<StatChange> healthRegenChange = new List<StatChange>();
    
    private List<StatChange> lifestealChange = new List<StatChange>();
    private List<StatChange> lifestealMultiplierChange = new List<StatChange>();

    private void Awake()
    {
        CheckSingleton();

        // Get base stats
        baseMoveSpeed = moveSpeed;
        baseMaxJumps = maxJumps;

        baseAttackDamage = attackDamage;
        baseAttackMultiplier = attackMultiplier;
        baseAttackCooldown = attackCooldown;
        baseCritChance = critChance;
        baseCritDamage = critDamage;
        baseLowerDamageVariance = lowerDamageVariance;
        baseUpperDamageVariance = upperDamageVariance;
        baseMaxAttacks = maxAttacks;
        
        baseMaxHealth = maxHealth;
        baseHealthRegen = healthRegen;
        baseRegenDelay = regenDelay;

        baseLifesteal = lifesteal;
        baseLifestealMultiplier = lifestealMultiplier;
    }

    // Adds a move speed stat modifier
    public void AddMoveSpeed(string name, float modifier)
    {
        AddStatChange(moveSpeedChange, name, modifier);
        moveSpeed = RecalculateStatAdditive(baseMoveSpeed, moveSpeedChange);
    }

    // Adds a max jump stat modifier
    public void AddMaxJump(string name, float modifier)
    {
        AddStatChange(maxJumpChange, name, modifier);
        maxJumps = (int)RecalculateStatAdditive(baseMaxJumps, maxJumpChange);
        PlayerController.Instance.movement.jumpsLeft = maxJumps;
    }



    // Adds an attack stat modifier
    public void AddAttack(string name, float modifier)
    {
        AddStatChange(attackChange, name, modifier);
        attackDamage = RecalculateStatAdditive(baseAttackDamage, attackChange);
    }

    // Adds an attack multiplier stat modifier
    public void AddAttackMultiplier(string name, float modifier)
    {
        AddStatChange(attackMultiplierChange, name, modifier);
        attackMultiplier = RecalculateStatAdditive(baseAttackMultiplier, attackMultiplierChange);
    }

    // Adds an attack cooldown stat modifier
    public void AddAttackCooldownReduction(string name, float modifier)
    {
        AddStatChange(attackCooldownChange, name, modifier);
        attackCooldown = RecalculateStatSubtractive(baseAttackCooldown, attackCooldownChange);
    }

    // Adds a crit chance stat modifier
    public void AddCritChange(string name, float modifier)
    {
        AddStatChange(critChanceChange, name, modifier);
        critChance = RecalculateStatAdditive(baseCritChance, critChanceChange);
    }

    // Adds a crit damage stat modifier
    public void AddCritDamage(string name, float modifier)
    {
        AddStatChange(critDamageChange, name, modifier);
        critDamage = RecalculateStatAdditive(baseCritDamage, critDamageChange);
    }

    // Adds a lower attack variance stat modifier
    public void AddLowerAttackVariance(string name, float modifier)
    {
        AddStatChange(upperDamageVarianceChange, name, modifier);
        lowerDamageVariance = RecalculateStatSubtractive(baseLowerDamageVariance, upperDamageVarianceChange);
    }

    // Adds an upper attack variance stat modifier
    public void AddUpperAttackVariance(string name, float modifier)
    {
        AddStatChange(lowerDamageVarianceChange, name, modifier);
        upperDamageVariance = RecalculateStatAdditive(baseUpperDamageVariance, upperDamageVarianceChange);
    }

    // Adds a max attack stat modifier
    public void AddMaxAttack(string name, float modifier)
    {
        AddStatChange(maxAttackChange, name, modifier);
        maxAttacks = (int)RecalculateStatAdditive(baseMaxAttacks, maxAttackChange);
    }



    // Adds a max health stat modifier
    public void AddMaxHealth(string name, float modifier)
    {
        AddStatChange(maxHealthChange, name, modifier);
        maxHealth = RecalculateStatAdditive(baseMaxHealth, maxHealthChange);
        PlayerController.Instance.health.SetMaxHealth(maxHealth);
    }

    // Adds a heath regen stat modifier
    public void AddHealthRegen(string name, float modifier)
    {
        AddStatChange(healthRegenChange, name, modifier);
        healthRegen = RecalculateStatAdditive(baseHealthRegen, healthRegenChange);
    }



    // Adds a lifesteal multiplier stat modifier
    public void AddLifesteal(string name, float modifier)
    {
        AddStatChange(lifestealChange, name, modifier);
        lifesteal = RecalculateStatAdditive(baseLifesteal, lifestealChange);
    }

    // Adds a lifesteal multiplier stat modifier
    public void AddLifestealMultiplier(string name, float modifier)
    {
        AddStatChange(lifestealMultiplierChange, name, modifier);
        lifestealMultiplier = RecalculateStatAdditive(baseLifestealMultiplier, lifestealMultiplierChange);
    }

    // Adds a StatIncrease to a given list by trying to find and modify an existing one, creating a new one if one is not found
    private void AddStatChange(List<StatChange> list, string name, float modifier)
    {
        foreach (StatChange stat in list)
        {
            if (name == stat.name)
            {
                stat.modifier = modifier;
                return;
            }
        }

        list.Add(new StatChange(name, modifier));
    }

    // Recalculates a stat by adding all the StatIncreases in a list to the base stat
    private float RecalculateStatAdditive(float baseStat, List<StatChange> list)
    {
        float finalStat = baseStat;
        foreach (StatChange statIncrease in list)
        {
            finalStat += statIncrease.modifier;
        }
        return finalStat;
    }

    // Recalculates a stat by subtracting all the StatIncreases in a list to the base stat
    private float RecalculateStatSubtractive(float baseStat, List<StatChange> list)
    {
        float finalStat = baseStat;
        foreach (StatChange statIncrease in list)
        {
            finalStat -= statIncrease.modifier;
        }
        return finalStat;
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

public class StatChange
{
    public string name;
    public float modifier;

    public StatChange(string _name, float _modifier)
    {
        name = _name;
        modifier = _modifier;
    }
}