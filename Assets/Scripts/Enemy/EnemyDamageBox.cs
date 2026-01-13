using UnityEngine;

public class EnemyDamageBox : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float hitDamage;

    private void Start()
    {
        hitDamage *= DifficultyHandler.Instance.difficulty;
    }

    // Damages player when they are in the trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.Instance.health.DamageWithKnockback(hitDamage, transform.position);
        }
    }
}
