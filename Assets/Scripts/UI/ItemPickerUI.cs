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
    // UI References
    public List<ItemButtonUI> buttons = new List<ItemButtonUI>();
    public GameObject LevelUpImage;
    public GameObject LevelUpParticleSystem;
    public float expandTime;
    public float expandScale;
    public float shrinkTime;
    public float shrinkScale;

    private void Start()
    {
        CloseUI();
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
        LevelUpParticleSystem.gameObject.SetActive(true);
        LevelUpImage.gameObject.SetActive(true);
        LevelUpImage.gameObject.transform.localScale = Vector3.zero;

        float elapsedTime = 0;
        while (elapsedTime < expandTime)
        {
            float textScale = Mathf.SmoothStep(0, expandScale, (elapsedTime / expandTime));
            elapsedTime += Time.unscaledDeltaTime;
            LevelUpImage.gameObject.transform.localScale = new Vector3(textScale, textScale, textScale);
            yield return null;
        }

        elapsedTime = 0;
        while (elapsedTime < shrinkTime)
        {
            float textScale = Mathf.SmoothStep(expandScale, shrinkScale, (elapsedTime / shrinkTime));
            elapsedTime += Time.unscaledDeltaTime;
            LevelUpImage.gameObject.transform.localScale = new Vector3(textScale, textScale, textScale);
            yield return null;
        }

        AssignRandomItemToButtons();

        yield return new WaitForSecondsRealtime(0.5f);
        
        LevelUpImage.gameObject.SetActive(false);
        ActivateButtons();
    }

    // Closes the item select UI
    public void CloseUI()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        DeactivateButtons();
        LevelUpParticleSystem.gameObject.SetActive(false);
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