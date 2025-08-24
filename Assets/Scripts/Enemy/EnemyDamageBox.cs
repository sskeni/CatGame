using UnityEngine;

public class EnemyDamageBox : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float hitDamage;

    // Damages player when they are in the trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.Instance.playerHealth.DamageWithKnockback(hitDamage, transform.position);
        }
    }
}
