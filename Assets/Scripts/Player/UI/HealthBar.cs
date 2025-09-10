using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // UI References
    private Slider healthSlider;

    void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (PlayerController.Instance != null) UpdateHealthVariables(PlayerController.Instance.health);
    }

    // Updates the health bar UI
    private void UpdateHealthVariables(PlayerHealth playerHealth)
    {
        healthSlider.maxValue = PlayerStats.Instance.maxHealth;
        healthSlider.value = playerHealth.currentHealth;
    }
}
