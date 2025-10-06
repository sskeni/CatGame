using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public virtual int maxStack() { return -1; }

    public abstract string GiveName();

    public abstract string GiveDescription();

    public virtual Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/TempItemImage");
    }

    public virtual void Update(PlayerController controller, int stacks) { }

    public virtual void AddPlayerStats(PlayerStats stats, int stacks) { }

    public virtual void OnHit(PlayerStats stats, IDamageable damageable, float damage, int stacks) { }
}

// Increases attack
public class SharperClaws : Item
{
    public override string GiveName()
    {
        return "Sharper Claws";
    }

    public override string GiveDescription()
    {
        return "Increases attack damage";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/SharperClaws");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = 0.25f * stacks;
        stats.AddStat(GiveName(), modifier, Stat.AttackDamage);
    }
}

// Increases movement speed
public class CatNip : Item
{
    public override string GiveName()
    {
        return "Cat Nip";
    }

    public override string GiveDescription()
    {
        return "Increases movement speed";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/CatNip");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = 5f * stacks;
        stats.AddStat(GiveName(), modifier, Stat.MoveSpeed);
    }
}

// Increases max health
public class CatFood : Item
{
    public override string GiveName()
    {
        return "Cat Food";
    }

    public override string GiveDescription()
    {
        return "Increases max health";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/CatFood");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = 1f * stacks;
        stats.AddStat(GiveName(), modifier, Stat.MaxHealth);
        PlayerController.Instance.health.FullyHeal();
    }
}

// Increases health regeneration rate
public class CatTreat : Item
{
    public override string GiveName()
    {
        return "Cat Treat";
    }

    public override string GiveDescription()
    {
        return "Increases health regeneration rate";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/CatTreat");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = 0.5f * stacks;
        stats.AddStat(GiveName(), modifier, Stat.HealthRegen);
    }
}

// Decreases attack speed
public class Milk : Item
{
    public override int maxStack()
    {
        return 5;
    }

    public override string GiveName()
    {
        return "Milk";
    }

    public override string GiveDescription()
    {
        return "Increases attack speed";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Milk");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = -0.1f  * stacks;
        stats.AddStat(GiveName(), modifier, Stat.AttackCooldown);
    }
}

// Increases critical chance
public class LionMane : Item
{
    public override int maxStack()
    {
        return 6;
    }

    public override string GiveName()
    {
        return "Lion Mane";
    }

    public override string GiveDescription()
    {
        return "Increases crit chance";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/LionMane");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = 15f * stacks;
        stats.AddStat(GiveName(), modifier, Stat.CritChance);
    }
}

// Increases critical damage
public class LionClaw : Item
{
    public override string GiveName()
    {
        return "Lion Claw";
    }

    public override string GiveDescription()
    {
        return "Increases crit damage";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/LionClaw");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = 25f * stacks;
        stats.AddStat(GiveName(), modifier, Stat.CritDamage);
    }
}

// Increases the amount of jumps the player can do before landing
public class Slinky : Item
{
    public override string GiveName()
    {
        return "Slinky";
    }

    public override string GiveDescription()
    {
        return "Increases jumps";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Slinky");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = stacks;
        stats.AddStat(GiveName(), modifier, Stat.MaxJumps);
        PlayerController.Instance.movement.jumpsLeft = stats.maxJumps;
    }
}

// Adds a small chance to do 100% of attack damage as an extra hit
public class LuckyDice : Item
{
    public override int maxStack()
    {
        return 5;
    }

    public override string GiveName()
    {
        return "Lucky Dice";
    }

    public override string GiveDescription()
    {
        return "Adds a small chance to do 100% of attack damage as an extra hit";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/LuckyDice");
    }

    public override void OnHit(PlayerStats stats, IDamageable damageable, float damage, int stacks)
    {
        int procChance = 5 * stacks;

        if (Random.Range(1, 101) <= procChance)
        {
            damageable.Damage(damage, false);
        }
    }
}

// Adds lifesteal to attacks
public class VampireFangs : Item
{
    public override int maxStack()
    {
        return 5;
    }

    public override string GiveName()
    {
        return "Vampire Fangs";
    }

    public override string GiveDescription()
    {
        return "Adds lifesteal to attacks";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/VampireFangs");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = 0.1f * stacks;
        stats.AddStat(GiveName(), modifier, Stat.Lifesteal);
    }

    public override void OnHit(PlayerStats stats, IDamageable damageable, float damage, int stacks)
    {
        float healAmount = damage * stats.lifesteal * stats.lifestealMultiplier;
        if (!stats.lifestealDoesDamage)
        {
            PlayerController.Instance.health.Heal(healAmount);
        }
        else
        {
            damageable.Damage(healAmount, false);
        }
    }
}

// Increases max attacks
public class KungFuTraining : Item
{
    public override int maxStack()
    {
        return 2;
    }

    public override string GiveName()
    {
        return "Kung Fu Training";
    }

    public override string GiveDescription()
    {
        return "Increases max attacks. Allows attacking multiple times in a row.";
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = stacks;
        stats.AddStat(GiveName(), modifier, Stat.MaxAttacks);
    }
}

// Lifesteal does damage
public class Mirror : Item
{
    public override int maxStack()
    {
        return 1;
    }

    public override string GiveName()
    {
        return "Mirror";
    }

    public override string GiveDescription()
    {
        return "Lifesteal does damage instead";
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        stats.lifestealDoesDamage = true;
    }
}

// Doubles lifesteal effect, but can no longer passively regen health
public class BloodThirst : Item
{
    public override int maxStack()
    {
        return 1;
    }

    public override string GiveName()
    {
        return "Blood Thirst";
    }

    public override string GiveDescription()
    {
        return "Lifesteal effect is doubled, but can no longer passively regen health";
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = stacks;
        stats.AddStat(GiveName(), modifier, Stat.LifestealMultiplier);
        PlayerController.Instance.health.canPassivelyRegen = false;
    }
}

// All attacks now do a random amount of damage from 75% to 150%
public class FuzzyDice : Item
{
    public override int maxStack()
    {
        return 1;
    }

    public override string GiveName()
    {
        return "Fuzzy Dice";
    }

    public override string GiveDescription()
    {
        return "All damage now does a random amount of damage from 75% to 150%";
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float lower = -0.25f;
        float upper = 0.5f;
        stats.AddStat(GiveName(), lower, Stat.LowerDamageVariance);
        stats.AddStat(GiveName(), upper, Stat.UpperDamageVariance);
    }
}

// Increases attack damage multiplier
public class MetalClaws : Item
{
    public override string GiveName()
    {
        return "Metal Claws";
    }

    public override string GiveDescription()
    {
        return "Increases attack damage multiplier";
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        float modifier = stacks * 0.10f;
        stats.AddStat(GiveName(), modifier, Stat.AttackMultiplier);
    }
}