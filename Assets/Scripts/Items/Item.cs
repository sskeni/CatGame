using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class Item
{
    public virtual int maxStack() { return -1; }

    public abstract string GiveName();

    public abstract string GiveDescription();

    public abstract Sprite GiveSprite();

    public virtual void Update(PlayerController controller, int stacks) { }

    public virtual void AddPlayerStats(PlayerStats stats, int stacks) { }

    public virtual void OnHit(PlayerStats stats, IDamageable damageable, int stacks) { }
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
        stats.attackDamage = stats.baseAttackDamage + (0.25f * stacks);
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
        stats.speed = stats.baseSpeed + (5f * stacks);
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
        stats.maxHealth = stats.baseMaxHealth + (1f * stacks);
        PlayerController.Instance.health.SetMaxHealth(stats.maxHealth);
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
        stats.regenRate = stats.baseRegenRate + (0.5f * stacks);
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
        stats.attackCooldown = stats.baseAttackCooldown - (stats.baseAttackCooldown * 0.1f  * stacks);
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
        stats.critChance = stats.baseCritChance + (15f * stacks);
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
        stats.critDamage = stats.baseCritDamage + (25f * stacks);
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
        stats.maxJumps = 1 + stacks;
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

    public override void OnHit(PlayerStats stats, IDamageable damageable, int stacks)
    {
        int procChance = 5 * stacks;

        if (Random.Range(1, 101) <= procChance)
        {
            damageable.Damage(stats.attackDamage, false);
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

    public override void OnHit(PlayerStats stats, IDamageable damageable, int stacks)
    {
        float healAmount = stats.attackDamage * 0.10f * stacks;
        PlayerController.Instance.health.Heal(healAmount);
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

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/TempItemImage");
    }

    public override void AddPlayerStats(PlayerStats stats, int stacks)
    {
        stats.maxAttacks = 1 + stacks;
    }
}