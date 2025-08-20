using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemUIManager : MonoBehaviour
{
    private static ItemUIManager instance;

    public static ItemUIManager Instance { get { return instance; } }

    public List<ItemButton> buttons = new List<ItemButton>();

    private void Start()
    {
        CheckSingleton();
        CloseUI();
    }

    // Setup singleton
    private void CheckSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Opens the item select UI
    public void OpenUI()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(OpenUICoroutine());
    }

    // Closes the item select UI
    public void CloseUI()
    {
        this.gameObject.SetActive(false);
        DeactivateButtons();
    }

    // Sets buttons to interactable
    public void ActivateButtons()
    {
        foreach (ItemButton button in buttons)
        {
            button.gameObject.SetActive(true);
            button.uiButton.interactable = true;
        }
    }

    // Sets buttons to uninteractable
    public void DeactivateButtons()
    {
        foreach (ItemButton button in buttons)
        {
            button.gameObject.SetActive(true);
            button.uiButton.interactable = false;
        }
    }

    public void AssignRandomItemToButtons()
    {
        foreach (ItemButton button in buttons)
        {
            // Get random item
            Array items = Enum.GetValues(typeof(Items));
            Items item = (Items)items.GetValue(Random.Range(0, items.Length));

            // Assign item
            button.SetItem(AssignItem(item));
        }
    }

    public Item AssignItem(Items itemToAssign)
    {
        switch (itemToAssign)
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

    private IEnumerator OpenUICoroutine()
    {
        AssignRandomItemToButtons();

        yield return new WaitForSeconds(0.5f);
        ActivateButtons();
    }
}

public enum Items
{
    SharperClaws,
    CapNip,
    CatFood,
    CatTreat,
    Milk,
    LionMane,
    LionClaw,
    Slinky,
    LuckyDice,
    VampireFangs
}