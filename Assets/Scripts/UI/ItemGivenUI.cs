using System.Collections;
using UnityEngine;

public class ItemGivenUI : MonoBehaviour
{
    private static ItemGivenUI instance;
    public static ItemGivenUI Instance { get { return instance; } }

    public ItemUI itemUI;
    public float delayBeforeClosing;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        CheckSingleton();
    }

    public void CheckSingleton()
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
