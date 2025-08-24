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

    public virtual void AddPlayerStats(PlayerController controller, int stacks) { }

    public virtual void OnHit(PlayerController controller, IDamageable damageable, int stacks) { }
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
        return "Increases attack";
    }

    public override Sprite GiveSprite()
    {
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void AddPlayerStats(PlayerController controller, int stacks)
    {
        controller.attackDamage = controller.baseAttackDamage + (1f * stacks);
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
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void AddPlayerStats(PlayerController controller, int stacks)
    {
        controller.speed = controller.baseSpeed + (5f * stacks);
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
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void AddPlayerStats(PlayerController controller, int stacks)
    {
        controller.playerHealth.SetMaxHealth(controller.playerHealth.baseMaxHealth + (5f * stacks));
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
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void AddPlayerStats(PlayerController controller, int stacks)
    {
        controller.playerHealth.regenRate = controller.playerHealth.baseRegenRate + (0.5f * stacks);
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
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void AddPlayerStats(PlayerController controller, int stacks)
    {
        controller.attackCooldown = controller.baseAttackCooldown - (controller.baseAttackCooldown * 0.1f  * stacks);
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
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void AddPlayerStats(PlayerController controller, int stacks)
    {
        controller.critChance = controller.baseCritChance + (15f * stacks);
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
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void AddPlayerStats(PlayerController controller, int stacks)
    {
        controller.critDamage = controller.baseCritDamage + (10f * stacks);
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
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void AddPlayerStats(PlayerController controller, int stacks)
    {
        controller.maxJumps = 1 + stacks;
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
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void OnHit(PlayerController controller, IDamageable damageable, int stacks)
    {
        int procChance = 5 * stacks;

        if (Random.Range(1, 101) <= procChance)
        {
            damageable.Damage(controller.attackDamage, false);
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
        return (Sprite)Resources.Load("Item Images/TempItemImage", typeof(Sprite));
    }

    public override void OnHit(PlayerController controller, IDamageable damageable, int stacks)
    {
        float healAmount = controller.attackDamage * 0.10f * stacks;
        controller.playerHealth.Heal(healAmount);
    }
}