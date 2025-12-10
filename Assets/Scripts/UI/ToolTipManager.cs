using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ToolTipManager : MonoBehaviour
{
    // Singleton References
    private static ToolTipManager instance;
    public static ToolTipManager Instance { get { return instance; } }

    // UI Refences
    [SerializeField] private RectTransform canvasRectTransform;
    public TextMeshProUGUI textUI;
    public float positionOffset;
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
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 targetPos = new Vector3(mousePos.x + positionOffset, mousePos.y + positionOffset, 0f);
        //rect.position = targetPos;

        //Vector3 screenPoint = Mouse.current.position.ReadValue() / canvasRectTransform.localScale.x;
        //screenPoint.z = 10.0f;
        //transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        //rect.anchoredPosition = Mouse.current.position.ReadValue();

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 targetPos = new Vector3(mousePos.x + positionOffset, mousePos.y + positionOffset, 0f);
        transform.position = targetPos;
    }
}
