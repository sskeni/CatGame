using UnityEngine;

public class LaserEnemyProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float hitDamage;
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        Move();
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
            if (hit.transform.tag == "Player")
            {
                hit.collider.gameObject.GetComponent<PlayerHealth>().DamageWithKnockback(hitDamage, transform.position);
                Destroy(this.gameObject);
            }
        }
    }
    
    // Move projectile forward
    private void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
