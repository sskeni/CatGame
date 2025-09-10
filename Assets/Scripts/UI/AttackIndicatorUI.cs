using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackIndicatorUI : MonoBehaviour
{
    public float xOffset;
    public float yOffset;
    public Slider slider;
    public TextMeshProUGUI text;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        FollowMouse();
        UpdateSlider();
    }

    // Follow the mouse position
    private void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = new Vector3(mousePos.x + xOffset, mousePos.y + yOffset, 0f);
        rect.position = targetPos;
    }

    // Updates the slider values and the text
    private void UpdateSlider()
    {
        float cooldownPercentage = PlayerController.Instance.attack.GetCooldownPercentage();
        int attacks = PlayerController.Instance.attack.GetAttacks();
        int maxAttacks = PlayerStats.Instance.maxAttacks;

        if (cooldownPercentage == 0 && attacks == maxAttacks)
        {
            slider.gameObject.SetActive(false);
        }
        else
        {
            slider.gameObject.SetActive(true);
            slider.value = cooldownPercentage;
        }

        if (maxAttacks > 1 && attacks > 0)
        {
            text.text = attacks.ToString();
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}
