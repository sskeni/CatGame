using UnityEngine;

public class EnemyDamageBox : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float hitDamage;

    private void FixedUpdate()
    {
        DamageCheck();
    }

    private void DamageCheck()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            origin: transform.position,
            size: transform.localScale,
            angle: transform.rotation.eulerAngles.z,
            direction: transform.up,
            distance: 0,
            layerMask: playerLayer);

        foreach (RaycastHit2D hit in hits)
        {
            PlayerHealth player = hit.collider.gameObject.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.DamageWithKnockback(hitDamage, transform.position);
            }
        }
    }
}
