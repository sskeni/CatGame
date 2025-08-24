using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemUIManager : MonoBehaviour
{
    // Singleton References
    private static ItemUIManager instance;
    public static ItemUIManager Instance { get { return instance; } }

    // UI References
    public List<ItemButton> buttons = new List<ItemButton>();

    // Private References
    private List<Item> itemPool = new List<Item>();

    private void Start()
    {
        CheckSingleton();
        CloseUI();
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
    private Item GetItemFromPool()
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

    // Opens the item select UI
    public void OpenUI()
    {
        PlayerController.Instance.DisablePlayControls();
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        StartCoroutine(OpenUICoroutine());
    }

    // Open UI Coroutine
    private IEnumerator OpenUICoroutine()
    {
        AssignRandomItemToButtons();

        yield return new WaitForSecondsRealtime(0.5f);
        ActivateButtons();
    }

    // Closes the item select UI
    public void CloseUI()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        DeactivateButtons();
        PlayerController.Instance.EnablePlayControls();
    }

    // Sets buttons to interactable
    public void ActivateButtons()
    {
        foreach (ItemButton button in buttons)
        {
            button.gameObject.SetActive(true);
            button.uiButton.interactable = true;
        }
    }

    // Sets buttons to uninteractable
    public void DeactivateButtons()
    {
        foreach (ItemButton button in buttons)
        {
            button.gameObject.SetActive(true);
            button.uiButton.interactable = false;
        }
    }

    // Assigns a random item to each button
    public void AssignRandomItemToButtons()
    {
        foreach (ItemButton button in buttons)
        {
            // Assign item
            button.SetItem(GetItemFromPool());
        }
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