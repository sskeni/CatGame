using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float fadeSpeed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(100f, 120f)));
    }

    void FixedUpdate()
    {
        text.alpha -= fadeSpeed;
        if (text.alpha <= 0f) Destroy(this.gameObject);
    }

    public void SetDamageAmount(float damage)
    {
        text.text = damage.ToString();
    }

    public void DamageWasCrit(bool wasCrit)
    {
        if (wasCrit)
        {
            text.color = Color.red;
            text.fontSize *= 1.5f;
        }
    }
}
