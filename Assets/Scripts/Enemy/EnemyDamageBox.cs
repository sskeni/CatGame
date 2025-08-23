using UnityEngine;

public class EnemyDamageBox : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float hitDamage;

    // Damages player when they enter the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.Instance.playerHealth.DamageWithKnockback(hitDamage, transform.position);
        }
    }
}
