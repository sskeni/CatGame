using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public Items itemDrop;

    void Start()
    {
        item = AssignItem(itemDrop);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.inventory.AddItem(item);
            Destroy(this.gameObject);
        }
    }

    public Item AssignItem(Items itemToAssign)
    {
        switch(itemToAssign)
        {
            case Items.SharperClaws:
                return new SharperClaws();
            case Items.CapNip:
                return new CatNip();
            case Items.CatFood:
                return new CatFood();
            case Items.CatTreat:
                return new CatTreat();
            case Items.Milk:
                return new Milk();
            case Items.LionMane:
                return new LionMane();
            case Items.LionClaw:
                return new LionClaw();
            case Items.Slinky:
                return new Slinky();
            case Items.LuckyDice:
                return new LuckyDice();
            case Items.VampireFangs:
                return new VampireFangs();
            default:
                return new SharperClaws();
        }
    }
}