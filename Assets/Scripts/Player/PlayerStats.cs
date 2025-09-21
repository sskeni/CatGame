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

    private List<StatChange> attackDamageChange = new List<StatChange>();
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

    public void AddStat(string name, float modifier, Stat statType)
    {
        switch (statType)
        {
            case Stat.MoveSpeed:
                moveSpeed = ChangeStat(baseMoveSpeed, moveSpeedChange, name, modifier);
                break;
            case Stat.MaxJumps:
                maxJumps = ChangeStat(baseMaxJumps, maxJumpChange, name, modifier);
                break;
            case Stat.AttackDamage:
                attackDamage = ChangeStat(baseAttackDamage, attackDamageChange, name, modifier);
                break;
            case Stat.AttackMultiplier:
                attackMultiplier = ChangeStat(baseAttackMultiplier, attackMultiplierChange, name, modifier);
                break;
            case Stat.AttackCooldown:
                attackCooldown = ChangeStat(baseAttackCooldown, attackCooldownChange, name, modifier);
                break;
            case Stat.CritChance:
                critChance = ChangeStat(baseCritChance, critChanceChange, name, modifier);
                break;
            case Stat.CritDamage:
                critDamage = ChangeStat(baseCritDamage, critDamageChange, name, modifier);
                break;
            case Stat.LowerDamageVariance:
                lowerDamageVariance = ChangeStat(baseLowerDamageVariance, lowerDamageVarianceChange, name, modifier);
                break;
            case Stat.UpperDamageVariance:
                upperDamageVariance = ChangeStat(baseUpperDamageVariance, upperDamageVarianceChange, name, modifier);
                break;
            case Stat.MaxAttacks:
                maxAttacks = ChangeStat(baseMaxAttacks, maxAttackChange, name, modifier);
                break;
            case Stat.MaxHealth:
                maxHealth = ChangeStat(baseMaxHealth, maxHealthChange, name, modifier);
                break;
            case Stat.HealthRegen:
                healthRegen = ChangeStat(baseHealthRegen, healthRegenChange, name, modifier);
                break;
            case Stat.Lifesteal:
                lifesteal = ChangeStat(baseLifesteal, lifestealChange, name, modifier);
                break;
            case Stat.LifestealMultiplier:
                lifestealMultiplier = ChangeStat(baseLifestealMultiplier, lifestealMultiplierChange, name, modifier);
                break;
            default:
                break;
        }
    }

    // Creates and adds a new StatIncrease and returns the calculated final stat
    private float ChangeStat(float baseStat, List<StatChange> statChanges, string name, float modifier)
    {
        AddStatChange(statChanges, name, modifier);
        return RecalculateStat(baseStat, statChanges);
    }
    private int ChangeStat(int baseStat, List<StatChange> statChanges, string name, float modifier)
    {
        AddStatChange(statChanges, name, modifier);
        return (int)RecalculateStat(baseStat, statChanges);
    }

    // Adds a StatIncrease to a given list by trying to find and modify an existing one, creating a new one if one is not found
    private void AddStatChange(List<StatChange> statChanges, string name, float modifier)
    {
        foreach (StatChange stat in statChanges)
        {
            if (name == stat.name)
            {
                stat.modifier = modifier;
                return;
            }
        }

        statChanges.Add(new StatChange(name, modifier));
    }

    // Recalculates a stat by adding all the StatIncreases in a list to the base stat
    private float RecalculateStat(float baseStat, List<StatChange> statChanges)
    {
        float finalStat = baseStat;
        foreach (StatChange statIncrease in statChanges)
        {
            finalStat += statIncrease.modifier;
        }
        return finalStat;
    }
    private int RecalculateStat(int baseStat, List<StatChange> statChanges)
    {
        int finalStat = baseStat;
        foreach (StatChange statIncrease in statChanges)
        {
            finalStat += (int)statIncrease.modifier;
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

public enum Stat
{
    MoveSpeed,
    JumpForce,
    MaxJumps,
    AttackDamage,
    AttackMultiplier,
    AttackCooldown,
    CritChance,
    CritDamage,
    LowerDamageVariance,
    UpperDamageVariance,
    MaxAttacks,
    MaxHealth,
    HealthRegen,
    DamageIFrames,
    KnockbackVelocity,
    Lifesteal,
    LifestealMultiplier
}