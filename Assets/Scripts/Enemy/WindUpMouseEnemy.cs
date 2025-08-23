using UnityEngine;

public class WindUpMouseEnemy: MonoBehaviour
{
    // Component References
    [SerializeField] private Rigidbody2D rb;

    // Move Stats
    [SerializeField] private float moveSpeed;

    // Private References
    private GameObject playerRef;

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerRef = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerRef = null;
        }
    }

    // Move enemy towards player
    private void Move()
    {
        if (playerRef != null)
        {
            if(playerRef.transform.position.x < transform.position.x) // Move right
            {
                rb.AddForce(new Vector2(-moveSpeed, 0f));
            }
            else // Move left
            {
                rb.AddForce(new Vector2(moveSpeed, 0f));
            }
        }
    }
}
