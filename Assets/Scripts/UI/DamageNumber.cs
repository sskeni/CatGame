using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float floatSpeed;
    [SerializeField] private float fadeSpeed;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y + floatSpeed, rectTransform.position.z);
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
