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
        enemy.onEnemyDamaged += new OnEnemyDamagedEventHandler(UpdateHealth);
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity; // Keep the UI upright
    }

    // Updates health bar
    private void UpdateHealth(float health)
    {
        healthSlider.value = health;
    }
}
