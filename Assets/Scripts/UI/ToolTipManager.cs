using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipManager : MonoBehaviour
{
    // Singleton References
    private static ToolTipManager instance;
    public static ToolTipManager Instance { get { return instance; } }

    // UI Refences
    public TextMeshProUGUI textUI;

    private void Awake()
    {
        CheckSingleton();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.position = Input.mousePosition;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    // Sets up Singleton
    private void CheckSingleton()
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

    // Shows and sets the tool tip text to a given string
    public void SetAndShowToolTip(string text)
    {
        gameObject.SetActive(true);
        textUI.text = text;
    }
    
    // Hides the tool tip
    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textUI.text = string.Empty;
    }
}
