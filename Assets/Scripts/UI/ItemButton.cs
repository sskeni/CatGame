using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Item item;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Button uiButton;

    private void Awake()
    {
        uiButton = GetComponent<Button>();
    }

    public void SetItem(Item newItem)
    {
        print(newItem.GiveName());
        itemName.text = newItem.GiveName();
        itemDescription.text = newItem.GiveDescription();
        item = newItem;
    }

    public void GiveItem()
    {
        PlayerController.Instance.playerInventory.AddItem(item);
    }
}
