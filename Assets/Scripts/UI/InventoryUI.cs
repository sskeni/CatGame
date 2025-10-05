using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    // Singleton References
    private static InventoryUI instance;
    public static InventoryUI Instance {  get { return instance; } }

    // Prefab Instances
    public GameObject itemPrefab;
    public Transform basePosition;

    // Private References
    List<GameObject> itemUIs = new List<GameObject>();
    
    private void Awake()
    {
        CheckSingleton();
    }

    private void Update()
    {
        UpdateUI();
    }

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

    // Updates the ToolTip UI based on if the mouse is hovering over a ItemUI
    private void UpdateUI()
    {
        ItemUI itemUI = IsPointerOverUIElement();
        if (itemUI != null)
        {
            ToolTipManager.Instance.SetAndShowToolTip(itemUI.ToolTipText());
        }
        else
        {
            ToolTipManager.Instance.HideToolTip();
        }
    }

    // Adds an item to UI
    public void AddItemUI(ItemList itemList)
    {
        CreateItemUI(itemList, itemUIs.Count);
    }

    // Updates stack count of an item, adds one if none exists
    public void UpdateItemUIStack(ItemList itemList)
    {
        foreach (GameObject item in itemUIs)
        {
            ItemUI itemUI = item.GetComponent<ItemUI>();
            if (itemList.item.GiveName() == itemUI.itemList.item.GiveName())
            {
                itemUI.UpdateStackCount(itemList.stacks);
                return;
            }
        }
        AddItemUI(itemList);
    }

    // Creates a new ItemUI with a given item
    public void CreateItemUI(ItemList itemList, int xOffset)
    {
        GameObject itemObject = Instantiate(
                itemPrefab,
                Vector3.zero,
                Quaternion.identity);
        itemObject.transform.SetParent(basePosition.transform);
        itemObject.GetComponent<RectTransform>().anchoredPosition = new Vector3((125f * xOffset), 0f, 0f);
        itemObject.transform.localScale = Vector3.one;

        ItemUI itemUI = itemObject.GetComponent<ItemUI>();
        itemUI.SetItem(itemList);

        itemUIs.Add(itemObject);
    }

    // Checks if the mouse is over a UI Element and returns the ItemUI it's hovering over
    private ItemUI IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> eventSystemRaycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, eventSystemRaycastResults);

        for (int i = 0; i < eventSystemRaycastResults.Count; i++)
        {
            RaycastResult curRaycastResult = eventSystemRaycastResults[i];
            if (curRaycastResult.gameObject.layer == LayerMask.NameToLayer("ItemUI"))
            {
                return curRaycastResult.gameObject.GetComponent<ItemUI>();
            }
        }
        return null;
    }
}
