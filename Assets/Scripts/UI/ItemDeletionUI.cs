using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDeletionUI : MonoBehaviour
{
    [SerializeField] private GameObject itemButton;
    [SerializeField] private GameObject itemPanel;

    void Start()
    {
        CloseUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void OpenUI()
    {
        PlayerController.Instance.DisablePlayControls();
        PlayerController.Instance.EnableUIControls();
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        LevelUIManager.Instance.inventory.CloseUI();
        DeleteAllItemButtons();
        CreateItemButtons();
    }

    public void CloseUI()
    {
        PlayerController.Instance.DisableUIControls();
        PlayerController.Instance.EnablePlayControls();
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        LevelUIManager.Instance.inventory.OpenUI();
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

    private void DeleteAllItemButtons()
    {
        foreach (Transform child in itemPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateItemButtons()
    {
        foreach (ItemList item in PlayerInventory.Instance.items)
        {
            GameObject newButton = Instantiate(itemButton, itemPanel.transform);
            ItemUI itemUI = newButton.GetComponent<ItemUI>();
            itemUI.SetItem(item);
            ItemDeletionButtonUI itemDeletionButton = newButton.GetComponent<ItemDeletionButtonUI>();
            itemDeletionButton.SetItem(item.item);
        }
    }

    public void RefreshUI()
    {
        LevelUIManager.Instance.inventory.CloseUI();
        DeleteAllItemButtons();
        CreateItemButtons();
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
