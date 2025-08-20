using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    private Slider healthSlider;

    void Start()
    {
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = enemy.maxHealth;
        healthSlider.value = enemy.currentHealth;
    }

    void FixedUpdate()
    {
        healthSlider.value = enemy.currentHealth;
    }
}
