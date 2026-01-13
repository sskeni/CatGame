using UnityEngine;

public class LaserEnemyProjectile : MonoBehaviour
{
    // Laser Projectile Stats
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float hitDamage;
    [SerializeField] private float speed;

    private void Start()
    {
        hitDamage *= DifficultyHandler.Instance.difficulty;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamagePlayer(collision);
        DestroyProjectile(collision);
    }

    // Damage when hitting the player
    private void DamagePlayer(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController.Instance.health.DamageWithKnockback(hitDamage, transform.position);
        }
    }

    // Destroy when hitting the player or the ground
    private void DestroyProjectile(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
    
    // Move projectile forward
    private void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

}
