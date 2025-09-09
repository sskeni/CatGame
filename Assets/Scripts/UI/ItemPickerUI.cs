using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemPickerUI: MonoBehaviour
{
    // Singleton References
    private static ItemPickerUI instance;
    public static ItemPickerUI Instance { get { return instance; } }

    // UI References
    public List<ItemButtonUI> buttons = new List<ItemButtonUI>();

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
        PlayerController.Instance.DisablePlayControls();
        PlayerController.Instance.EnableUIControls();
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        StartCoroutine(OpenUICoroutine());
    }

    // Open UI Coroutine
    private IEnumerator OpenUICoroutine()
    {
        AssignRandomItemToButtons();

        yield return new WaitForSecondsRealtime(0.5f);
        ActivateButtons();
    }

    // Closes the item select UI
    public void CloseUI()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        DeactivateButtons();
        PlayerController.Instance.EnablePlayControls();
        PlayerController.Instance.DisableUIControls();
    }

    // Sets buttons to interactable
    public void ActivateButtons()
    {
        foreach (ItemButtonUI button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }

    // Sets buttons to uninteractable
    public void DeactivateButtons()
    {
        foreach (ItemButtonUI button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    // Assigns a random item to each button
    public void AssignRandomItemToButtons()
    {
        foreach (ItemButtonUI button in buttons)
        {
            // Assign item
            button.SetItem(ItemPoolManager.Instance.GetItemFromPool());
        }
    }
}