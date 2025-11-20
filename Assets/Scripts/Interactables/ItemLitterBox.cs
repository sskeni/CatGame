using TMPro;
using UnityEngine;

public class ItemLitterBox : Interactable
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

    // Checks if the player has enough money to buy the chest
    private bool CanPay()
    {
        return PlayerCoins.Instance.CoinCount() >= cost;
    }

    public override void Interact()
    {
        if (interactable && CanPay() && !bought && !PlayerInventory.Instance.IsEmpty())
        {
            PlayerCoins.Instance.RemoveCoins(cost);
            bought = true;
            uiPrompt.SetActive(false);

            LevelUIManager.Instance.itemDeletion.OpenUI();
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
