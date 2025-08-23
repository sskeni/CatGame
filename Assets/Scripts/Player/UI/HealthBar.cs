using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;

    void Awake()
    {
        healthSlider = GetComponent<Slider>();
        PlayerController.Instance.playerHealth.OnHealthChanged += new OnHealthChangedEventHandler(UpdateHealthVariables);
    }

    private void UpdateHealthVariables(float maxHealth, float currentHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
