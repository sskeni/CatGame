using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    private Slider healthSlider;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = enemy.maxHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity; // Keep the UI upright
    }

    public void SetCurrentHealth(float health)
    {
        healthSlider.value = health;
    }

    public void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
    }
}
