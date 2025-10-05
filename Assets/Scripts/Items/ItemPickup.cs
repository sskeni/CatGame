using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public Items itemDrop;

    void Start()
    {
        item = ItemPoolManager.Instance.AssignItem(itemDrop);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInventory.Instance.AddItem(item);
            Destroy(this.gameObject);
        }
    }
}