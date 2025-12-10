using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemChest : Interactable
{
    // UI References
    [SerializeField] private GameObject uiPrompt;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Sprite usedSprite;
    
    // Public References
    public int cost;

    // Private References
    private bool interactable = false;
    private bool bought = false;

    public override void Start()
    {
        base.Start();
        uiPrompt.SetActive(false);
        costText.text = string.Format("x {0}", cost);
    }

    // Checks if the player has enough money to buy the chest
    private bool CanPay()
    {
        return PlayerCoins.Instance.CoinCount() >= cost;
    }

    // Buys the chest, giving the player a random item, also disables the chest from being able to be bought again
    public override void Interact()
    {
        if (interactable && CanPay() && !bought)
        {
            PlayerCoins.Instance.RemoveCoins(cost);

            // Give a random item from the item pool
            Item ItemToGive = ItemPoolManager.Instance.GetItemFromPool();
            PlayerInventory.Instance.AddItem(ItemToGive);

            LevelUIManager.Instance.itemGiven.SetItem(ItemToGive);
            LevelUIManager.Instance.itemGiven.OpenUI();

            // Keep track of chest count
            RunStatisticsHandler.Instance.totalChestsOpened++;

            GetComponent<SpriteRenderer>().sprite = usedSprite;

            bought = true;
            uiPrompt.SetActive(false);
        }
    }

    protected override void EnterTrigger(Collider2D collision)
    {
        if (!bought)
        {
            uiPrompt.SetActive(true);
            interactable = true;
        }
    }

    protected override void ExitTrigger(Collider2D collision)
    {
        uiPrompt.SetActive(false);
        interactable = false;
    }
}