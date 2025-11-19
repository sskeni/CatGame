using System.Collections;
using UnityEngine;

public class ItemGivenUI : MonoBehaviour
{
    public ItemUI itemUI;
    public float delayBeforeClosing;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public void SetItem(Item item)
    {
        ItemList itemList = new ItemList(item, item.GiveName(), 0);
        itemUI.SetItem(itemList);
    }

    public void OpenUI()
    {
        this.gameObject.SetActive(true);
        PlayerController.Instance.DisablePlayControls();
        PlayerController.Instance.EnableUIControls();
    }

    public void CloseUI()
    {
        this.gameObject.SetActive(false);
        PlayerController.Instance.EnablePlayControls();
        PlayerController.Instance.DisableUIControls();
    }
}
