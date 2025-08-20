using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemList> items = new List<ItemList>();

    public void AddItem(Item item)
    {
        foreach (ItemList i in items)
        {
            if (i.name == item.GiveName())
            {
                i.stacks += 1;
                i.item.AddPlayerStats(PlayerController.Instance, i.stacks);
                return;
            }
        }
        items.Add(new ItemList(item, item.GiveName(), 1));
        item.AddPlayerStats(PlayerController.Instance, 1);
    }
}
