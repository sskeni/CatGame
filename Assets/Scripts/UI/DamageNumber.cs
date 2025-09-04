using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    // UI References
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float fadeSpeed;

    // Private References
    private Color critColor;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ColorUtility.TryParseHtmlString("#a83f48", out critColor);
    }

    private void Start()
    {
        rb.AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(100f, 120f))); // Sets random velocity for damage number
    }

    void FixedUpdate()
    {
        FadeUI();
    }

    // Fades the UI
    private void FadeUI()
    {
        text.alpha -= fadeSpeed;
        if (text.alpha <= 0f) Destroy(this.gameObject);
    }

    // Sets the UI to a given damage amount
    public void SetDamageAmount(float damage)
    {
        text.text = damage.ToString();
    }

    // Changes the text color and size if the damage was a crit
    public void DamageWasCrit(bool wasCrit)
    {
        if (wasCrit)
        {
            text.color = critColor;
            text.fontSize *= 1.5f;
        }
    }
}
