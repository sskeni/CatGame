using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    // UI References
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float fadeSpeed;

    // Parameters
    public Color critColor;
    public Color healColor;
    public float initialForce;
    public float initialYForceVariance;

    // Private References
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Sets random velocity for damage number
        float xForce = Random.Range(-initialForce, initialForce);
        float yForce = Random.Range(initialForce, initialForce + initialYForceVariance);
        rb.AddForce(new Vector2(xForce, yForce));
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
        float roundedDamage = Mathf.Round(damage * 100) / 100;
        text.text = roundedDamage.ToString();
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

    public void WasHeal(bool wasCrit)
    {
        text.color = healColor;
        if (wasCrit)
        {
            text.fontSize *= 1.5f;
        }
    }
}
