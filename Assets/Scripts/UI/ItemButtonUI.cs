using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonUI : MonoBehaviour
{
    // Item Reference
    public Item item;

    // UI References
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;
    public Button uiButton;

    private void Awake()
    {
        uiButton = GetComponent<Button>();
    }

    // Set the item for the button and update UI
    public void SetItem(Item newItem)
    {
        itemName.text = newItem.GiveName();
        itemDescription.text = newItem.GiveDescription();
        itemImage.sprite = newItem.GiveSprite();
        item = newItem;
    }

    // Gives the player the item set to this button
    public void GiveItem()
    {
        PlayerInventory.Instance.AddItem(item);
    }
}
