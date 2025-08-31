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
    private RectTransform rect;

    private void Awake()
    {
        CheckSingleton();
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        FollowMouse();
    }

    private void FixedUpdate()
    {
        FollowMouse();
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

    // Makes the tool tip follow the mouse
    private void FollowMouse()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        rect.position = targetPos;
        //rect.anchoredPosition = Input.mousePosition;
    }
}
