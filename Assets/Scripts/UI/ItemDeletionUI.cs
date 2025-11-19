using UnityEngine;

public class ItemDeletionUI : MonoBehaviour
{
    [SerializeField] private GameObject itemButton;
    [SerializeField] private GameObject itemPanel;

    void Start()
    {
        CreateItemButtons();
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
        }
    }

    public void RefreshUI()
    {
        DeleteAllItemButtons();
        CreateItemButtons();
    }
}
