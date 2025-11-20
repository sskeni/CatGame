using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    private static PlayerInventory instance;
    public static PlayerInventory Instance { get { return instance; } }

    // Inventory References
    public List<ItemList> items = new List<ItemList>();

    private void Awake()
    {
        CheckSingleton();
        SceneManager.sceneLoaded += ReloadAllItems;
    }

    // Set up singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Applies stat changes from all items to PlayerStats
    public void ReloadAllItems(Scene scene, LoadSceneMode mode)
    {
        LevelUIManager.Instance.inventory.DeleteAllItemUI();
        foreach (ItemList i in items)
        {
            i.item.AddPlayerStats(PlayerStats.Instance, i.stacks);
            LevelUIManager.Instance.inventory.AddItemUI(i);
        }
    }

    [ContextMenu("Reload All Items")]
    public void ReloadAllItems()
    {
        PlayerStats.Instance.ResetAllStatsToBase();
        PlayerStats.Instance.RemoveAllStatChanges();

        LevelUIManager.Instance.inventory.DeleteAllItemUI();

        foreach (ItemList i in items)
        {
            i.item.AddPlayerStats(PlayerStats.Instance, i.stacks);
            LevelUIManager.Instance.inventory.AddItemUI(i);
        }
    }

    // Adds an item to the inventory and updates UI
    public void AddItem(Item item)
    {
        foreach (ItemList i in items)
        {
            if (i.name == item.GiveName())
            {
                i.stacks += 1;
                i.item.AddPlayerStats(PlayerStats.Instance, i.stacks);
                LevelUIManager.Instance.inventory.UpdateItemUIStack(i);
                return;
            }
        }
        ItemList itemToAdd = new ItemList(item, item.GiveName(), 1);
        items.Add(itemToAdd);
        LevelUIManager.Instance.inventory.AddItemUI(itemToAdd);
        item.AddPlayerStats(PlayerStats.Instance, 1);
    }

    public void RemoveItem(Item item)
    {
        foreach (ItemList i in items)
        {
            if (i.name == item.GiveName())
            {
                items.Remove(i);
                break;
            }
        }
        ReloadAllItems();
    }

    // Returns the number of stacks of the given item
    public int GetStack(Item itemToCheck)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemToCheck.GiveName())
            {
                return i.stacks;
            }
        }

        return 0;
    }

    public bool IsEmpty()
    {
        return items.Count == 0;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ReloadAllItems;
    }
}
