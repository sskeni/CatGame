using UnityEngine;

public class EnemyAutoJump : MonoBehaviour
{
    // Component References
    [SerializeField] private Rigidbody2D rb;

    // Jump Stats
    [SerializeField] private float jumpForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Jump();
    }

    // Make the enemy jump
    private void Jump()
    {
        rb.linearVelocityY = 0;
        rb.AddForceY(jumpForce);
    }
}
