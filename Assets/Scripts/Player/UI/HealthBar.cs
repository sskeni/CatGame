using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private Slider healthSlider;

    void Start()
    {
        playerHealth = PlayerController.Instance.playerHealth;
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.playerHealth;
    }

    void FixedUpdate()
    {
        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.playerHealth;
    }
}
