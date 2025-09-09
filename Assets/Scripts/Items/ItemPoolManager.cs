using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemPoolManager : MonoBehaviour
{
    // Singleton References
    private static ItemPoolManager instance;
    public static ItemPoolManager Instance { get { return instance; } }

    // Private References
    private List<Item> itemPool = new List<Item>();

    private void Awake()
    {
        CheckSingleton();
        PopulateItemPool();
    }

    // Setup singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Fills the item pool with all items
    private void PopulateItemPool()
    {
        Array items = Enum.GetValues(typeof(Items));
        for (int i = 0; i < items.Length; i++)
        {
            itemPool.Add(AssignItem((Items)items.GetValue(i)));
        }
    }

    // Returns an item from the item pool
    // Removes items that have reached max stacks
    public Item GetItemFromPool()
    {
        // Get a random item
        Item item = itemPool[Random.Range(0, itemPool.Count)];

        // Check if the item has a max stack
        if (item.maxStack() != -1)
        {
            // Check if player item stack is less than max stack
            if (PlayerController.Instance.playerInventory.GetStack(item) < item.maxStack())
            {
                return item;
            }
            else // If the player has max stacks, remove item from pool and try again
            {
                itemPool.Remove(item);
                return GetItemFromPool();
            }
        }

        return item;
    }

    // Returns Item type from Enum Items
    public Item AssignItem(Items itemToAssign)
    {
        switch (itemToAssign)
        {
            case Items.SharperClaws:
                return new SharperClaws();
            case Items.CapNip:
                return new CatNip();
            case Items.CatFood:
                return new CatFood();
            case Items.CatTreat:
                return new CatTreat();
            case Items.Milk:
                return new Milk();
            case Items.LionMane:
                return new LionMane();
            case Items.LionClaw:
                return new LionClaw();
            case Items.Slinky:
                return new Slinky();
            case Items.LuckyDice:
                return new LuckyDice();
            case Items.VampireFangs:
                return new VampireFangs();
            default:
                return new SharperClaws();
        }
    }
}

// Enum to hold all item types
public enum Items
{
    SharperClaws,
    CapNip,
    CatFood,
    CatTreat,
    Milk,
    LionMane,
    LionClaw,
    Slinky,
    LuckyDice,
    VampireFangs
}