using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // UI References
    private Slider healthSlider;

    void Awake()
    {
        healthSlider = GetComponent<Slider>();
        PlayerController.Instance.playerHealth.OnHealthChanged += new OnHealthChangedEventHandler(UpdateHealthVariables);
    }

    // Updates the health bar UI
    private void UpdateHealthVariables(float maxHealth, float currentHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
