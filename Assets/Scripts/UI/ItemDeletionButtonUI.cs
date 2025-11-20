using UnityEngine;

public class ItemDeletionButtonUI : MonoBehaviour
{
    Item item;

    public void SetItem(Item newItem)
    {
        item = newItem;
    }

    public void DeleteItem()
    {
        PlayerInventory.Instance.RemoveItem(item);
        LevelUIManager.Instance.itemDeletion.CloseUI();
    }
}
