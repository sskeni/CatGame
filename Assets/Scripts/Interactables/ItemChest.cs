using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemChest : Interactable
{
    // UI References
    [SerializeField] private GameObject uiPrompt;
    [SerializeField] private TextMeshProUGUI costText;
    
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !bought)
        {
            uiPrompt.SetActive(true);
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            uiPrompt.SetActive(false);
            interactable = false;
        }
    }

    // Checks if the player has enough money to buy the chest
    private bool CanPay()
    {
        return PlayerCoins.Instance.CoinCount() >= cost;
    }

    // Buys the chest, giving the player a random item, also disables the chest from being able to be bought again
    public override void Interact()
    {
        if (interactable && CanPay())
        {
            PlayerCoins.Instance.RemoveCoins(cost);

            // Give a random item from the item pool
            Item ItemToGive = ItemPoolManager.Instance.GetItemFromPool();
            PlayerInventory.Instance.AddItem(ItemToGive);

            ItemGivenUI.Instance.SetItem(ItemToGive);
            ItemGivenUI.Instance.OpenUI();

            // Keep track of chest count
            RunStatisticsHandler.Instance.totalChestsOpened++;

            bought = true;
            uiPrompt.SetActive(false);
        }
    }
}