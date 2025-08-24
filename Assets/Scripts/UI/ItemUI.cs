using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    // UI References
    public Image image;
    public TextMeshProUGUI stackCountText;
    public ItemList itemList;

    // Sets the UI to a given item at 0 stacks
    public void SetItem(ItemList newItem)
    {
        itemList = newItem;
        image.sprite = itemList.item.GiveSprite();
        stackCountText.text = string.Empty;
    }

    // Sets the stack count of the UI
    public void UpdateStackCount(int newStacks)
    {
        stackCountText.text = "x" + newStacks;
    }

    // Returns the hover over string for the tool tip
    public string ToolTipText()
    {
        string message =
            itemList.item.GiveName() + "\n" + 
            itemList.item.GiveDescription();
        return message;
    }
}
