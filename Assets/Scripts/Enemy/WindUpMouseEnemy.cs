using UnityEngine;

public class WindUpMouseEnemy: MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    private GameObject playerRef;

    private void FixedUpdate()
    {
        Move();
    }

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
}
